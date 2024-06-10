using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Passion_Project.Models;

namespace Passion_Project.Controllers
{
    public class RecipeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RecipeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44399/api/RecipeData/");
        }
        /// <summary>
        /// Lists all recipes.
        /// </summary>
        /// <returns>A view with a list of RecipeDto objects.</returns>
        // GET: Recipe/List
        public ActionResult List()
        {
            string url = "ListRecipes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RecipeDto> Recipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;
            //Access data for list recipes
            return View(Recipes);
        }

        /// <summary>
        /// Shows details of a specific recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to show.</param>
        /// <returns>A view with the RecipeDto object.</returns>
        // Get: Recipe/Show/{id}
        public ActionResult Show(int id)
        {
            //access data for find recipe
            string url = "FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            // Get comments for the recipe
            string commentsUrl = "https://localhost:44399/api/CommentData/ListCommentsForRecipe/" + id;
            HttpResponseMessage commentsResponse = client.GetAsync(commentsUrl).Result;
            IEnumerable<CommentDto> Comments = commentsResponse.Content.ReadAsAsync<IEnumerable<CommentDto>>().Result;

            var viewModel = new RecipeCommentsViewModel
            {
                Recipe = Recipe,
                Comments = Comments
            };
            return View(viewModel);
        }

        /// <summary>
        /// Returns an error view.
        /// </summary>
        /// <returns>An error view.</returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Returns the view for creating a new recipe.
        /// </summary>
        /// <returns>A view for creating a new recipe.</returns>
        // GET: Recipe/New
        public ActionResult New()
        {
            return View();
        }


        /// <summary>
        /// Creates a new recipe.
        /// </summary>
        /// <param name="recipe">The Recipe object to create.</param>
        /// <returns>Redirects to the list of recipes on success, or an error view on failure.</returns>
        //POST: Recipe/Create
        [HttpPost]
        public ActionResult Create(Recipe recipe)
        {
            Debug.WriteLine("the json payload is :");

            string url = "AddRecipe";

            string jsonpayload = jss.Serialize(recipe);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Returns the view for editing an existing recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to edit.</param>
        /// <returns>A view with the RecipeDto object.</returns>
        //GET: Recipe/Edit/4
        public ActionResult Edit(int id)
        {
            string url = "FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(Recipe);
        }


        /// <summary>
        /// Updates an existing recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to update.</param>
        /// <param name="recipe">The updated Recipe object.</param>
        /// <returns>Redirects to the recipe details on success, or an error view on failure.</returns>
        //POST: Recipe/Update/3
        [HttpPost]
        public ActionResult Update(int id, Recipe recipe)
        {
            try
            {
                Debug.WriteLine("The new recipe info is:");
                Debug.WriteLine(recipe.RecipeName);
                Debug.WriteLine(recipe.RecipeIngredient);
                Debug.WriteLine(recipe.RecipeInstruction);
                Debug.WriteLine(recipe.RecipeAuthor);

                //serialize into JSON
                //Send the request to the API

                string url = "UpdateRecipe/" + id;

                string jsonpayload = jss.Serialize(recipe);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show", new { id = recipe.RecipeId });
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            catch
            {
                return View(recipe);
            }

        }

        /// <summary>
        /// Returns the view for confirming the deletion of a recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        /// <returns>A view with the RecipeDto object.</returns>
        //GET: Recipe/DeleteConfirm/3
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;
            return View(Recipe);
        }

        /// <summary>
        /// Deletes a recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        /// <returns>Redirects to the list of recipes on success, or an error view on failure.</returns>
        // POST: Recipe/Delete/3
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteRecipe/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}