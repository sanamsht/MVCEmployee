using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Models;

namespace MVCEmployee.Controllers
{
    public class CascadeController : Controller
    {
        private readonly AppDbContext _context;
        public CascadeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CascadeDropdown()
        {
            return View();
        }
        public JsonResult Country()
        {
            var data = _context.Countries.ToList();
            return new JsonResult(data);
        }
        public JsonResult State(int id)
        {
            var data = _context.States.Where(e=>e.Country.Id==id).ToList();
            return new JsonResult(data);
        }
        public JsonResult District(int id)
        {
            var data = _context.Districts.Where(e => e.State.Id == id).ToList();
            return new JsonResult(data);
        }
    }
}
