using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MVCEmployee.Models.ViewModel
{
    public class SignUpUserViewModel
    {
        public int Id { get; set; }
        [Remote(action: "CheckUserName", controller:"User")]
        [Required(ErrorMessage ="Username cannot be empty")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress(ErrorMessage ="Please Enter valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile cannot be empty")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter valid mobile number")]
        public long Mobile { get; set; }
        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please re-enter the password")]
        [Compare(nameof(Password), ErrorMessage ="Password Mismatch")]
        public string ConfirmPassword { get; set; }
        [Display(Name ="Active")]
        public bool IsActive { get; set; }
    }
}
