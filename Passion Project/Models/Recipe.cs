using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passion_Project.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        public string RecipeName { get; set; }

        public string RecipeIngredient { get; set; }

        public string RecipeInstruction { get; set; }

        //A Recipe is Posted by one user
        //A User can post many recipes
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string UserName { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }

    //A data transfer object (DTO)
    //Communicating the recipe information externally
    public class RecipeDto
    {
        public int RecipeId { get; set; }

        public string RecipeName { get; set; }

        public string RecipeIngredient { get; set; }

        public string RecipeInstruction { get; set; }

        public string RecipeAuthor { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
    }

}