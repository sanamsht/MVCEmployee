using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Models;
using System.Net.Http.Headers;

namespace MVCEmployee.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:52896");
      
        public IActionResult Index()
        {
            return View();
        }
       
        public async Task<IActionResult> GetEmployeeData()
        {
            List<Employee> modelList = new List<Employee>();
            using(var client =new HttpClient())
            {
                client.BaseAddress = baseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/employees");
            if (response.IsSuccessStatusCode)
            {
                var data =  response.Content.ReadAsStringAsync().Result;
                modelList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(data);
               
            }
            }
            return new JsonResult(modelList);
        }
    }
}
