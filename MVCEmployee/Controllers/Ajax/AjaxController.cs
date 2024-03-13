using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Models;

namespace MVCEmployee.Controllers.Ajax
{
    public class AjaxController : Controller
    {
        private readonly AppDbContext _context;
        public AjaxController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult DepartmentList()
        {
            var daata = _context.Departments.ToList();
            return new JsonResult(daata);
        }
        public JsonResult EmployeeList()
        {
            var data = _context.Employees.ToList();
            return new JsonResult(data);
        }
        [HttpPost]
        public JsonResult AddEmployee(Employee employee)
        {
            var emp = new Employee()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateofBirth = employee.DateofBirth.ToUniversalTime(),
                Gender = employee.Gender,
                DepartmentId = employee.DepartmentId,
                PhotoPath = employee.PhotoPath

            };
            _context.Employees.Add(emp);
            _context.SaveChanges();
            return new JsonResult("Employee Added Successfully");
        }
        public JsonResult Delete(int id)
        {
            var data = _context.Employees.Where(e => e.EmployeeId == id).FirstOrDefault();
            if(data != null)
            {
                _context.Employees.Remove(data);
                _context.SaveChanges();
                return new JsonResult("Record Deleted Successfully");
            }
            else
            { return new JsonResult("Failed to Delete Record"); }
        }

        public JsonResult Edit(int id)
        {
            var data = _context.Employees.Where(e => e.EmployeeId == id).FirstOrDefault();
            return new JsonResult(data); 
        }

        [HttpPost]
        public JsonResult Edit(Employee employee)
        {
            var data = _context.Employees.Where(e=>e.EmployeeId == employee.EmployeeId).FirstOrDefault();
            if(data != null)
            {
                data.FirstName = employee.FirstName;
                data.LastName = employee.LastName;
                data.Email = employee.Email;
                data.DateofBirth = employee.DateofBirth.ToUniversalTime();
                data.Gender = employee.Gender;
                data.DepartmentId = employee.DepartmentId;
                if (employee.PhotoPath != null)
                {
                    data.PhotoPath = employee.PhotoPath;
                }
                _context.Employees.Update(data);
                _context.SaveChanges();
                return new JsonResult("Employee Updated Successfully");
            }
            else
            {
                return new JsonResult("Update Failed");
            }
           
        }
    }
}
