using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
     public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
           
            var result = await _context.Employees.AddAsync(employee);
            
            return result.Entity;
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var result = await _context.Employees
                           .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            if (result != null)
            {
                _context.Employees.Remove(result);
                
            }
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            IQueryable<Employee> qry = _context.Employees;
            if(!string.IsNullOrEmpty(name))
            {
                qry = qry.Where(e=>e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
            if(gender !=null)
            {
                qry = qry.Where(e=>e.Gender == gender);
            }
            return await qry.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result =  _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if(result != null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Gender = employee.Gender;
                    result.Email = employee.Email;
                result.DateofBirth = employee.DateofBirth;
                if (employee.DepartmentId != 0)
                {
                    result.DepartmentId = employee.DepartmentId;
                }
                else if(employee.Department != null)
                {
                    result.DepartmentId = employee.Department.DepartmentId;
                }
                result.PhotoPath = employee.PhotoPath;

              
                return result;
            }
            return null;
        }
    }
}
