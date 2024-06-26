using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passion_Project.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public string CommentText { get; set; }

        public DateTime CommentTime { get; set; } = DateTime.Now;

        //A comment can be under one recipe
        //A recipe can have many comments
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }


        //A Comment is Posted by one user
        //A User can post many comments
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }

    public class CommentDto
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; }
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

    }
}