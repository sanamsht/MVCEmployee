using System.ComponentModel.DataAnnotations;

namespace MVCEmployee.Models.ViewModel
{
    public class LoginSignUpViewModel
    {
        [Required(ErrorMessage= "Please Enter Username")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Please Enter Password")]
       public string Password { get; set; }
       
   
        [Display(Name = "Remember Me")]
        public bool IsRemember { get; set; }
    }
}
