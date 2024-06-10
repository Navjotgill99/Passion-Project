using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Passion_Project.Models;
using System.Web.Script.Serialization;

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

        /// <summary>
        /// Displays the view for creating a new comment.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe to which the comment belongs.</param>
        /// <returns>The view for creating a new comment.</returns>
        // GET: Comment/New/{recipeId}
        public ActionResult New(int recipeId)
        {
            Comment comment = new Comment
            {
                RecipeId = recipeId,
                UserId = 2 //Assume logged-in user Id is 2 for now
            };
            
            return View(comment);
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">The Comment object to create.</param>
        /// <returns>Redirects to the recipe details page on success, or an error view on failure.</returns>
        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            comment.CommentTime = DateTime.Now;
            string url = "AddComment";

            string jsonpayload = jss.Serialize(comment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show", "Recipe", new { id = comment.RecipeId });
            }
            else
            {
                return RedirectToAction("Error", "Recipe");
            }
        }

        /// <summary>
        /// Displays the view for editing an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to edit.</param>
        /// <returns>The view for editing the comment.</returns>
        // GET: Comment/Edit/{id}
        public ActionResult Edit(int id)
        {
            string url = "FindComment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CommentDto comment = response.Content.ReadAsAsync<CommentDto>().Result;

            return View(comment);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="comment">The updated Comment object.</param>
        /// <returns>Redirects to the recipe details page on success, or an error view on failure.</returns>
        // POST: Comment/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(commentDto);
            }

            // Convert CommentDto to Comment model
            Comment comment = new Comment
            {
                CommentId = commentDto.CommentId,
                CommentText = commentDto.CommentText,
                CommentTime = commentDto.CommentTime,
                RecipeId = commentDto.RecipeId,
                UserId = commentDto.UserId
            };

            string url = "UpdateComment/" + id;

            string jsonpayload = jss.Serialize(comment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show", "Recipe", new { id = comment.RecipeId });
            }
            else
            {
                return RedirectToAction("Error", "Recipe");
            }
        }

        /// <summary>
        /// Returns the view for confirming the deletion of a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>A view with the CommentDto object.</returns>
        //GET: Comment/DeleteConfirm/{id}
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindComment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CommentDto comment = response.Content.ReadAsAsync<CommentDto>().Result;
            return View(comment);
        }

        /// <summary>
        /// Deletes a comment.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>Redirects to the recipe details page on success, or an error view on failure.</returns>
        // POST: Comment/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteComment/" + id;
            //HttpContent content = new StringContent("");
            //content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                CommentDto comment = response.Content.ReadAsAsync<CommentDto>().Result;
                return RedirectToAction("Show", "Recipe", new { id = comment.RecipeId });
            }
            else
            {
                return RedirectToAction("Error", "Recipe");
            }
        }
    }
}