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

        public DateTime CommentTime { get; set; }

        //A comment can be under one recipe
        //A recipe can have many comments
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }


        //Many users can comment many times
        [ForeignKey("User")]
        public int UserId {  get; set; }
        public virtual User User { get; set; }







    }
}