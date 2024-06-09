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
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Recipe/List
        public ActionResult List()
        {
            string url = "ListRecipes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RecipeDto> Recipes = response.Content.ReadAsAsync<IEnumerable<RecipeDto>>().Result;
            //access data for list recipes
            return View(Recipes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // Get: Recipe/Show/{id}
        public ActionResult Show(int id)
        {
            //access data for find recipe
            string url = "FindRecipe/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(Recipe);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Recipe/New
        public ActionResult New()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
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

        //GET: Recipe/Edit/4
        public ActionResult Edit(int id)
        {
            string url = "FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            return View(Recipe);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
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

                return RedirectToAction("Show/" + id);
            }
            catch
            {
                return View();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        //GET: Recipe/DeleteConfirm/2
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindRecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipeDto Recipe = response.Content.ReadAsAsync<RecipeDto>().Result;
            return View(Recipe);
        }

    }
}