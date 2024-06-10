using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Passion_Project.Models;

namespace Passion_Project.Controllers
{
    public class CommentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of comments for a specific recipe.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe.</param>
        /// <returns>An IEnumerable of CommentDto objects.</returns>
        [HttpGet]
        [Route("api/CommentData/ListCommentsForRecipe/{recipeId}")]
        public IEnumerable<CommentDto> ListCommentsForRecipe(int recipeId)
        {
            List<Comment> comments = db.Comments.Where(c => c.RecipeId == recipeId).ToList();
            List<CommentDto> commentDtos = new List<CommentDto>();

            comments.ForEach(c => commentDtos.Add(new CommentDto()
            {
                CommentId = c.CommentId,
                CommentText = c.CommentText,
                CommentTime = c.CommentTime,
                RecipeId = c.RecipeId,
                UserId = c.UserId
            }));

            return commentDtos;
        }

        /// <summary>
        /// Finds a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>An IHttpActionResult containing the CommentDto object if found, or NotFound response if not found.</returns>
        [HttpGet]
        [ResponseType(typeof(CommentDto))]
        [Route("api/CommentData/FindComment/{id}")]
        public IHttpActionResult FindComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            CommentDto commentDto = new CommentDto()
            {
                CommentId = comment.CommentId,
                CommentText = comment.CommentText,
                CommentTime = comment.CommentTime,
                RecipeId = comment.RecipeId,
                UserId = comment.UserId
            };

            return Ok(commentDto);
        }

        /// <summary>
        /// Adds a new comment.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>An IHttpActionResult indicating success or failure.</returns>
        [HttpPost]
        [Route("api/CommentData/AddComment")]
        public IHttpActionResult AddComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure CommentTime is within the valid range for SQL Server datetime
            if (comment.CommentTime < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                comment.CommentTime = DateTime.Now; // or set to a valid minimum value
            }
            db.Comments.Add(comment);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="comment">The updated comment.</param>
        /// <returns>An IHttpActionResult indicating success or failure.</returns>
        [HttpPost]
        [Route("api/CommentData/UpdateComment/{id}")]
        public IHttpActionResult UpdateComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            // Ensure CommentTime is within the valid range for SQL Server datetime
            if (comment.CommentTime < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                comment.CommentTime = DateTime.Now; // or set to a valid minimum value
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return InternalServerError(e);
            }
            

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>An IHttpActionResult indicating success or failure.</returns>
        [HttpPost]
        [Route("api/CommentData/DeleteComment/{id}")]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }
    }
}
