using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
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
        /// Lists all recipes in the system.
        /// </summary>
        /// <returns>A view with a list of RecipeDto objects.</returns>
        // GET: Recipe/List
        public ActionResult List()
        {
            //curl https://localhost:44399/api/recipedata/listrecipes

            string url = "ListRecipes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<RecipeDto> Recipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;
                Debug.WriteLine("Number of recipes in the list: " + Recipes.Count());
                //Access data for list recipes
                return View(Recipes);
            }
            else
            {
                Debug.WriteLine("Error retrieving recipes: " + response.ReasonPhrase);
                return RedirectToAction("Error");
            }
            
        }

        /// <summary>
        /// Shows details of a specific recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to display.</param>
        /// <returns>A view displaying the details of the RecipeDto object.</returns>
        // Get: Recipe/Show/{id}
        public ActionResult Show(int id)
        {
            //curl https://localhost:44399/api/recipedata/findrecipe/{id}


            //access data for find recipe
            string url = "FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            // Get comments for the recipe
            string commentsUrl = "https://localhost:44399/api/CommentData/ListCommentsForRecipe/" + id;
            HttpResponseMessage commentsResponse = client.GetAsync(commentsUrl).Result;
            IEnumerable<CommentDto> Comments = commentsResponse.Content.ReadAsAsync<IEnumerable<CommentDto>>().Result;

            RecipeCommentsViewModel viewModel = new RecipeCommentsViewModel
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
        [Authorize]
        public ActionResult New()
        {
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.Name;
            return View();
        }


        /// <summary>
        /// Creates a new recipe.
        /// </summary>
        /// <param name="recipe">The Recipe object to create.</param>
        /// <returns>Redirects to the list of recipes on success, or an error view on failure.</returns>
        //POST: Recipe/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Recipe recipe)
        {
            //curl -H "Content-Type:application/json" -d @recipe.json https://localhost:44399/api/recipedata/addrecipe

            recipe.UserId = User.Identity.GetUserId();
            recipe.UserName = User.Identity.Name;

            Debug.WriteLine("the json payload is :");

            string url = "AddRecipe";

            string jsonpayload = jss.Serialize(recipe);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Recipe created successfully.");
                return RedirectToAction("List");
            }
            else
            {
                Debug.WriteLine("Error creating recipe.");
                return View("Error");
            }
        }

        /// <summary>
        /// Returns the view for editing an existing recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to edit.</param>
        /// <returns>A view with the RecipeDto object.</returns>
        //GET: Recipe/Edit/4
        [Authorize]
        public ActionResult Edit(int id)
        {
            //curl https://localhost:44399/api/recipedata/findrecipe/{id}

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
        [Authorize]
        public ActionResult Update(int id, Recipe recipe)
        {
            try
            {
                Debug.WriteLine("The new recipe info is:");
                Debug.WriteLine(recipe.RecipeName);
                Debug.WriteLine(recipe.RecipeIngredient);
                Debug.WriteLine(recipe.RecipeInstruction);

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
        [Authorize]
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
        [Authorize]
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