using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForAnimalsApplication.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage ="Acest camp este obligatoriu!")]
        public string UserId { get; set; }
        [Required(ErrorMessage ="Acest camp este obligatoriu!")]
        public string RoleName { get; set; }
    }
}