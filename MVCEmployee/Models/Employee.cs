
using System.ComponentModel.DataAnnotations;



namespace MVCEmployee.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Please Enter Firstname")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Please Enter Lastname")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Select Date of Birth")]
        public DateTime DateofBirth { get; set; }
        [Required(ErrorMessage = "Please Select your Gender")]
        public Gender? Gender { get; set; }
        [Required(ErrorMessage = "Department Id cannot be empty")]
        public int? DepartmentId { get; set; }
        
        public string PhotoPath { get; set; }
       
        
    }
}
