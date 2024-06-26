using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Passion_Project.Models;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace Passion_Project.Controllers
{
    public class CommentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CommentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44399/api/CommentData/");
        }

        // GET: Comment/List
        public ActionResult List()
        {
            //curl https://localhost:44399/api/commentdata/listcomments

            string url = "listcomments";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CommentDto> Comments = response.Content.ReadAsAsync<IEnumerable<CommentDto>>().Result;

            return View(Comments);
        }

        //GET: Comment/Show/{id}
        public ActionResult Show(int id)
        {
            //curl https://localhost:44399/api/commentdata/findcomment/{id}

            string url = "findcomment/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            CommentDto Comment = response.Content.ReadAsAsync<CommentDto>().Result;

            return View(Comment);
        }

        public ActionResult Error()
        {
            return View();
        }


        /// <summary>
        /// Displays the view for creating a new comment.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe to which the comment belongs.</param>
        /// <returns>The view for creating a new comment.</returns>
        // GET: Comment/New/{recipeId}
        [Authorize]
        public ActionResult New(int recipeId)
        {
            CommentDto commentDto = new CommentDto
            {
                RecipeId = recipeId,
                UserId = User.Identity.GetUserId(),
                UserName = User.Identity.Name
            };

            return View(commentDto);
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">The Comment object to create.</param>
        /// <returns>Redirects to the recipe details page on success, or an error view on failure.</returns>
        // POST: Comment/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Comment comment)
        {
            comment.CommentTime = DateTime.Now;
            comment.UserId = User.Identity.GetUserId();
            comment.User.UserName = User.Identity.Name;

            string url = "AddComment";

            string jsonpayload = jss.Serialize(comment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            Debug.WriteLine("Request URL: " + url);
            Debug.WriteLine("Request Payload: " + jsonpayload);

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine("Response Status Code: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                CommentDto createdcomment = response.Content.ReadAsAsync<CommentDto>().Result;
                return RedirectToAction("Show", "Recipe", new { id = comment.RecipeId });
            }
            else
            {
                Debug.WriteLine("Error: Failed to create comment. Status Code: " + response.StatusCode);
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays the view for editing an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to edit.</param>
        /// <returns>The view for editing the comment.</returns>
        // GET: Comment/Edit/{id}
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "FindComment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CommentDto selectedComment = response.Content.ReadAsAsync<CommentDto>().Result;

            return View(selectedComment);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="comment">The updated Comment object.</param>
        /// <returns>Redirects to the recipe details page on success, or an error view on failure.</returns>
        // POST: Comment/Update/{id}
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, CommentDto commentDto)
        {
            commentDto.CommentTime = DateTime.Now;
            commentDto.UserId = User.Identity.GetUserId();
            commentDto.UserName = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return View(commentDto);
            }

           

            string url = "UpdateComment/" + id;

            string jsonpayload = jss.Serialize(commentDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show", "Recipe", new { id = commentDto.RecipeId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Returns the view for confirming the deletion of a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>A view with the CommentDto object.</returns>
        //GET: Comment/DeleteConfirm/{id}
        [HttpGet]
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindComment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                CommentDto comment = response.Content.ReadAsAsync<CommentDto>().Result;
                return View(comment);
            }
            else
            {
                // Handle case where comment retrieval fails
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Deletes a comment.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>Redirects to the recipe details page on success, or an error view on failure.</returns>
        // POST: Comment/Delete/{id}
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "DeleteComment/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                CommentDto comment = response.Content.ReadAsAsync<CommentDto>().Result;
                return RedirectToAction("Show", "Recipe", new { id = comment.RecipeId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}