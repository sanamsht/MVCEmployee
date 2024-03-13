using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Models;
using MVCEmployee.Models.ViewModel;
using System.Security.Claims;
using System.Text;

namespace MVCEmployee.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateDNTCaptcha(ErrorMessage ="Wrong Captcha! Please Try Again")]
        public IActionResult Login(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (user != null)
                {
                    bool isValid = (user.Username == model.Username && user.Password == hashPassword(model.Password));
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Username) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", model.Username);

                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        TempData["errorPassword"] = "Invalid Password";
                        return View(model);
                    }
                }
                else
                {
                    TempData["errorUsername"] = "Invalid Username";
                    return View(model);
                }
            }
            else
            {
                TempData["errorMessage"] = "Please Enter Username and Password to login!";
                return View(model);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach(var key in storedCookies)
            {
                Response.Cookies.Delete(key);
            }
            return RedirectToAction("Login");

        }
        public IActionResult SignUp()
        {
            return View();

        }
        [HttpPost]
        public IActionResult SignUp(SignUpUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Password = hashPassword(model.Password),
                    IsActive = model.IsActive,

                };
                _context.Users.Add(data);
                _context.SaveChanges();
                TempData["successMessage"] = "User Successfully Created, Please fill your credential's to login!";
                return RedirectToAction("Login");

            }
            else
            {
                TempData["errorMessage"] = "*Error on submitting!";
                return View(model);
            }

        }
        [AcceptVerbs("Post", "Get")]
        public IActionResult CheckUserName(string userName)
        {
            var data = _context.Users.Where(e => e.Username == userName).FirstOrDefault();
            if(data !=null)
            {
                return Json($"{userName} already in use");
            }
            else
            {
                return Json(true);
            }
        }
        public static string hashPassword(string password)
        {
            byte[] passcode = ASCIIEncoding.ASCII.GetBytes(password);
            string hashPassword = Convert.ToBase64String(passcode);
            return hashPassword;
        }

        public static string decryptPassword(string password)
        {
            byte[] hashPassword = Convert.FromBase64String(password);
            string passcode = ASCIIEncoding.ASCII.GetString(hashPassword);
            
            return passcode;
        }
    }
}
