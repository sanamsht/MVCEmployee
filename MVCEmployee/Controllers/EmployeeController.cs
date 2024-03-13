using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Models;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Runtime.CompilerServices;

namespace MVCEmployee.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;


        public EmployeeController(AppDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public IActionResult Index()
        {
            var result = _context.Employees.ToList();
            return View(result);
        }

        public IActionResult Create()
        {
            ViewData["Department"] = _context.Departments.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee, IFormFile Photopath)
        {
            ViewData["Department"] = _context.Departments.ToList();
            var filePath = "";
            if (Photopath != null)
            {
                var path = Path.Combine(_environment.WebRootPath, "Images");
                filePath = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                var fullPath = Path.Combine(path, filePath);
                UploadFile(Photopath, fullPath);
            }

            if (employee.EmployeeId == 0)
            {
                //New Employee Added here!!
                if (employee.FirstName != null && employee.LastName != null && employee.Email != null)
                {

                    var emp = new Employee()
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email,
                        DateofBirth = employee.DateofBirth.ToUniversalTime(),
                        Gender = employee.Gender,
                        DepartmentId = employee.DepartmentId,
                        PhotoPath = filePath
                    };
                    _context.Employees.Add(emp);
                    _context.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    return View(employee);
                }
            }
            else
            {
                //Employee Details Edited Here!!
                var data = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                if (data != null)
                {
                    data.FirstName = employee.FirstName;
                    data.LastName = employee.LastName;
                    data.Email = employee.Email;
                    data.DateofBirth = employee.DateofBirth.ToUniversalTime();
                    data.Gender = employee.Gender;
                    data.DepartmentId = employee.DepartmentId;
                    if (Photopath != null)
                    {
                        string existingFile = Path.Combine(_environment.WebRootPath, "Images", data.PhotoPath);
                        if(System.IO.File.Exists(existingFile))
                        { System.IO.File.Delete(existingFile); }

                        data.PhotoPath = filePath;
                    }

                    _context.Employees.Update(data);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }




        public void UploadFile(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);

        }

        public IActionResult Edit(int id)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            ViewData["Department"] = _context.Departments.ToList();
            var result = new Employee()
            {
                EmployeeId = emp.EmployeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                DateofBirth = emp.DateofBirth,
                Gender = emp.Gender,
                DepartmentId = emp.DepartmentId,
                PhotoPath = emp.PhotoPath
            };
            return View("Create", result);
        }



        public IActionResult Delete(int id)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
