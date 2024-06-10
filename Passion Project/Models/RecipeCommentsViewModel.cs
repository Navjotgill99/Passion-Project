using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passion_Project.Models
{
    public class RecipeCommentsViewModel
    {
        public RecipeDto Recipe { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}