using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Passion_Project.Models;

namespace Passion_Project.Controllers
{
    public class RecipeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns a list of recipes in the system
        /// </summary>
        /// <returns>An array of RecipeDto objects.</returns>
        // <example>
        // GET: /api/RecipeData/ListRecipes -> [{"RecipeId":2,"RecipeName":"Chocolate Chip Cookies","RecipeIngredient":"- 200g flour - 100g sugar - - 200g chocolate chips" and so on,"RecipeInstruction":"1. Preheat oven to 175°C (350°F). 2. Mix butter, sugars, egg, and vanilla. 3. Add flour and baking soda, mix well." ,"RecipeAuthor":"Nav"}]
        //</example>
        [HttpGet]
        [Route("api/RecipeData/ListRecipes")]
        public List<RecipeDto> ListRecipes()
        {
            List<Recipe> Recipes = db.Recipes.ToList();

            List<RecipeDto> RecipeDtos = new List<RecipeDto>();

            Recipes.ForEach(r => RecipeDtos.Add(new RecipeDto()
            {
                RecipeId = r.RecipeId,
                RecipeName = r.RecipeName,
                RecipeIngredient = r.RecipeIngredient,
                RecipeInstruction = r.RecipeInstruction,
                RecipeAuthor = r.RecipeAuthor
            }));

            return RecipeDtos;
        }

        
        /// <summary>
        /// Returns one recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to find.</param>
        /// <returns>A singular RecipeDto object.</returns>
        [ResponseType(typeof(Recipe))]
        [HttpGet]
        [Route("api/RecipeData/FindRecipe/{id}")]
        public IHttpActionResult FindRecipe(int id)
        {
            Recipe Recipe = db.Recipes.Find(id);
            RecipeDto RecipeDto = new RecipeDto()
            {
                RecipeId = Recipe.RecipeId,
                RecipeName = Recipe.RecipeName,
                RecipeIngredient = Recipe.RecipeIngredient,
                RecipeInstruction = Recipe.RecipeInstruction,
                RecipeAuthor = Recipe.RecipeAuthor
            };
            if (Recipe == null)
            {
                return NotFound();
            }
            return Ok(RecipeDto);
        }

        /// <summary>
        /// Adds a new recipe to the system.
        /// </summary>
        /// <param name="recipe">The Recipe object to add.</param>
        /// <returns>HTTP response indicating success or failure.</returns>
        [ResponseType(typeof (Recipe))]
        [HttpPost]
        [Route("api/RecipeData/AddRecipe")]
        public IHttpActionResult AddRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Recipes.Add(recipe);
            db.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Updates an existing recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to update.</param>
        /// <param name="recipe">The updated Recipe object.</param>
        /// <returns>HTTP response indicating success or failure.</returns>
        [ResponseType(typeof (void))]
        [HttpPost]
        [Route("api/RecipeData/UpdateRecipe/{id}")]
        public IHttpActionResult UpdateRecipe(int id, Recipe recipe)
        {
            //Debug.WriteLine("I have reached the update recipe method!");
            if (!ModelState.IsValid)
            {
                //Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeId)
            {
                //Debug.WriteLine("Id Mismatch");
                //Debug.WriteLine("GET parameter" + id);
                //Debug.WriteLine("POST parameter" + recipe.RecipeId);
                //Debug.WriteLine("POST parameter" + recipe.RecipeName);
                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    //Debug.WriteLine("Recipe not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes a recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        /// <returns>HTTP response indicating success or failure.</returns>
        [ResponseType(typeof(Recipe))]
        [HttpPost]
        [Route("api/RecipeData/DeleteRecipe/{id}")]
        public IHttpActionResult DeleteRecipe(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipe);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Disposes of the database context.
        /// </summary>
        /// <param name="disposing">Whether the context is being disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Checks if a recipe exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to check.</param>
        /// <returns>True if the recipe exists, false otherwise.</returns>
        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(r => r.RecipeId == id) > 0;
        }
    }
}
