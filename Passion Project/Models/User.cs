using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passion_Project.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

    }
}