using Microsoft.AspNetCore.Mvc;
using ProjecPRN.Models;
using System.Text.Json;

namespace ProjecPRN.Controllers
{
    public class AccountController : Controller
    {


        private readonly CenimaDBContext _dbContext;
        public AccountController()
        {
            _dbContext = new CenimaDBContext();
        }



        [BindProperty]
        public Person person { get; set; }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("account") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Person p)
        {


            Person account = _dbContext.Persons.Where(acc => acc.Email == p.Email && acc.Password == p.Password)
                .SingleOrDefault();
            if (account != null)
            {
                if (account.IsActive == true)
                {
                    HttpContext.Session.SetString("account", JsonSerializer.Serialize(account));
                    if (account.Type == 1)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }

                else
                {
                    ViewBag.Error = $"Tài khoản {p.Email} của bạn đã bị cấm do vi phạm tiêu chuẩn cộng đồng!!!";
                    return View("Error");
                }
            }
            else
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng";
                return View();
            }
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("account") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(Person p)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Person account = _dbContext.Persons.Where(acc => acc.Email == p.Email)
            .SingleOrDefault();
            if (account != null)
            {
                ViewBag.Message = "Email đã tồn tại. Vui lòng chọn email khác";
                return View();
            }
            p.IsActive = true;
            p.Type = 2;
            _dbContext.Persons.Add(p);
            _dbContext.SaveChanges();
            HttpContext.Session.SetString("account", JsonSerializer.Serialize(p));
            return RedirectToAction("Index", "Home");
        }
        public IActionResult SignOut()
        {
            if (HttpContext.Session.GetString("account") != null)
                HttpContext.Session.Remove("account");
            return RedirectToAction("Index", "Home");
        }
    }
}
