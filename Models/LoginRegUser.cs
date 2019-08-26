using System;
using System.ComponentModel.DataAnnotations;

namespace belt.Models {
    public class LoginReg {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email {get;set;}

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
    }
}