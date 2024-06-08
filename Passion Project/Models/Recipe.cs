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

        public string RecipeAuthor { get; set; }
    }
}