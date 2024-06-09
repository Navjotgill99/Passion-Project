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
        /// <returns>An array of recipes</returns>
        // <example>
        // GET: /api/RecipeData/ListRecipes -> [{}]
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
        /// Returns one recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns> A singular recipe</returns>
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
        /// 
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        [ResponseType(typeof (void))]
        [HttpPost]
        [Route("api/RecipeData/UpdateRecipe/{id}")]
        public IHttpActionResult UpdateRecipe(int id, Recipe recipe)
        {
            Debug.WriteLine("I have reached the update recipe method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeId)
            {
                Debug.WriteLine("Id Mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + recipe.RecipeId);
                Debug.WriteLine("POST parameter" + recipe.RecipeName);
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
                    Debug.WriteLine("Recipe not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(r => r.RecipeId == id) > 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        
           

    }
}
