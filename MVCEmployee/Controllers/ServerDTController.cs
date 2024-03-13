using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Models;
using System.Linq.Dynamic.Core;

namespace MVCEmployee.Controllers
{
    [Authorize]
    public class ServerDTController : Controller
    {
        public readonly AppDbContext _context;
        public ServerDTController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = Request.Form;
                int pageSize = 0;
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault(); 
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumns = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault()+"][name]"].FirstOrDefault();
                var sortColunDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                var data = (from employee in _context.Employees select employee);
                if(!string.IsNullOrEmpty(sortColumns)&& !string.IsNullOrEmpty(sortColunDir))
                {
                    data = data.OrderBy(sortColumns + " " + sortColunDir);
                }
                if(!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(e=>e.FirstName.Contains(searchValue) || e.LastName.Contains(searchValue) || e.Email.Contains(searchValue));
                }
                int totalRecord = data.Count();
                var cData = data.Skip(skip).Take(pageSize).ToList();
                var jsonData = new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = cData

                };
                return new JsonResult(jsonData);
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }
    }
}
