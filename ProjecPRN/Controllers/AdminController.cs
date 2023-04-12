using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjecPRN.Controllers;
using ProjecPRN.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Runtime.ConstrainedExecution;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CenimaDBContext dBContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment environment;

        public AdminController(ILogger<HomeController> logger, CenimaDBContext dBContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            this.dBContext = dBContext;
            this.environment = environment;
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        public IActionResult Movies()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MoviesList = dBContext.Movies.Include(c => c.Genre).ToList();
            return View();
        }
        public IActionResult Persons()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Personlist = dBContext.Persons.ToList();
            return View();
        }
        public IActionResult Genres()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Genrelist = dBContext.Genres.ToList();
            return View();
        }
        public IActionResult AddMovie()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            var genres = dBContext.Genres.ToList();
            ViewData["Genres"] = genres;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie, IFormFile file, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (movie == null)
            {
                return RedirectToAction("AddMovie");
            }
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            if (file == null || file.Length == 0)
                return Content("No file selected.");
            var folderPath = Path.Combine(webHostEnvironment.WebRootPath, $"{movie.Title}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Lấy tên file và đường dẫn file tới thư mục upload
            var fileName = file.FileName;
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, $"{movie.Title}", fileName);



            // Lưu file vào đường dẫn đã chọn
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            dBContext.Add(movie);
            dBContext.SaveChanges();



            //ViewData["GenreId"] = new SelectList(dBContext.Genres, "GenreId", "Description");

            return RedirectToAction("Movies");
            //return View(movie);
        }
        public IActionResult AddGenres()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddGenre(Genre gen)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            dBContext.Genres.Add(gen);
            dBContext.SaveChanges();
            return RedirectToAction("Genres");
        }
        public IActionResult EditGenres(int id)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            var genres = dBContext.Genres.Find(id);
            if (genres != null)
            {
                return View(genres);
            }
            ViewData["message"] = $"GenresID: {id} không tồn tại.";
            return View("Error");
        }
        [HttpPost]
        public IActionResult EditGenres(Genre gen)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            if (gen == null)
            {
                ViewData["message"] = $"Thể loại phim này rỗng!!!";
                return View("Error");
            }
            var genEdit = dBContext.Genres.Find(gen.GenreId);
            genEdit.Description = gen.Description.Trim();
            dBContext.SaveChanges();
            return RedirectToAction("Genres");
        }
        public IActionResult DeleteGenre(int id)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Movie> ml = dBContext.Movies.Where(p => p.GenreId == id).ToList();
            if (ml.Count > 0)
            {
                ViewData["message"] = $"Không thể xoá thể loại phim {dBContext.Genres.Find(id).Description} vì trong thể loại phim này đang có phim!!.";
                return View("Error");
            }
            dBContext.Remove(dBContext.Genres.Find(id));
            dBContext.SaveChanges();
            var movieGenre = dBContext.Genres.ToList();
            return RedirectToAction("Genres");
        }
        public IActionResult ChangePersonStatus(int id)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person person = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (person.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            if (person.PersonId == id)
            {
                ViewData["message"] = $"Không thể cấm bản thân!!.";
                return View("Error");
            }
            var per = dBContext.Persons.Find(id);
            if (per.Type == 1)
            {
                ViewData["message"] = $"Không thể cấm hoặc hạ cấp người cùng cấp!!.";
                return View("Error");
            }


            bool status = (bool)dBContext.Persons.Find(id).IsActive;
            var p = dBContext.Persons.Find(id);
            p.IsActive = !status;
            dBContext.SaveChanges();
            return RedirectToAction("Persons");
        }
        public IActionResult ChangePersonType(int id)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person person = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (person.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            if (person.PersonId == id)
            {
                ViewData["message"] = $"Không thể hạ cấp bản thân!!.";
                return View("Error");
            }
            var per = dBContext.Persons.Find(id);
            if (per.Type == 1)
            {
                ViewData["message"] = $"Không thể cấm hoặc hạ cấp người cùng cấp!!.";
                return View("Error");
            }
            if (per.IsActive == false)
            {
                ViewData["message"] = $"Không thể thăng cấp người đang bị cấm, hãy gỡ cấm họ trước!!.";
                return View("Error");
            }
            int type = (int)dBContext.Persons.Find(id).Type;
            if (type == 1)
            {
                var p = dBContext.Persons.Find(id);
                p.Type = 2;
            }
            else if (type == 2)
            {
                var p = dBContext.Persons.Find(id);
                p.Type = 1;
            }
            dBContext.SaveChanges();
            return RedirectToAction("Persons");
        }

        public async Task<IActionResult> DeleteMovie(int id, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            var movie = await dBContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\{movie.Title}", movie.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var folderPath = Path.Combine(webHostEnvironment.WebRootPath, $"{movie.Title}");
            if (Directory.Exists(folderPath))
            {

                Directory.Delete(folderPath);
            }
            List<Rate> rate = dBContext.Rates.Where(c => c.MovieId == id).ToList();
            if (rate.Count > 0)
            {
                ViewData["message"] = $"Không thể xóa phim đã có đánh giá!!.";
                return View("Error");
            }
            dBContext.Movies.Remove(movie);
            await dBContext.SaveChangesAsync();

            return RedirectToAction("Movies");
        }
        [HttpPost]
        public async Task<IActionResult> EditMovie(Movie movie, IFormFile file, [FromServices] IWebHostEnvironment webHostEnvironment, string oldimage, string oldtitle)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            var m = dBContext.Movies.Find(movie.MovieId);
            if (file != null)
            {

                var filepath = Path.Combine(webHostEnvironment.WebRootPath, $"{oldtitle}", oldimage);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                var folderpath = Path.Combine(webHostEnvironment.WebRootPath, $"{oldtitle}");
                if (Directory.Exists(folderpath))
                {

                    Directory.Delete(folderpath);
                }
                var folderPath = Path.Combine(webHostEnvironment.WebRootPath, $"{movie.Title}");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = file.FileName;
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, $"{movie.Title}", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                movie.Image = fileName;
            }
            if (file == null)
            {

                // Đường dẫn folder chứa ảnh
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\{oldtitle}");

                // Đường dẫn tới file ảnh của movie
                var imagePath = Path.Combine(folderPath, oldimage);

                // Đọc dữ liệu ảnh từ file
                var imageData = System.IO.File.ReadAllBytes(imagePath);

                // Tạo đối tượng IFormFile để truyền vào view
                file = new FormFile(new MemoryStream(imageData), 0, imageData.Length, movie.Image, movie.Image);
                var filepath = Path.Combine(webHostEnvironment.WebRootPath, $"{oldtitle}", oldimage);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                var folderpath = Path.Combine(webHostEnvironment.WebRootPath, $"{oldtitle}");
                if (Directory.Exists(folderpath))
                {

                    Directory.Delete(folderpath);
                }
                var folderPaths = Path.Combine(webHostEnvironment.WebRootPath, $"{movie.Title}");
                if (!Directory.Exists(folderPaths))
                {
                    Directory.CreateDirectory(folderPaths);
                }
                var fileName = file.FileName;
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, $"{movie.Title}", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                movie.Image = fileName;

            }


            if (m != null)
            {
                m.Title = movie.Title;
                m.Year = movie.Year;
                m.Description = movie.Description;
                m.GenreId = movie.GenreId;

                if (file != null)
                {
                    m.Image = movie.Image;
                }

                dBContext.Update(m);
                dBContext.SaveChanges();
            }

            return RedirectToAction("Movies");
        }
        public ActionResult EditMovie(int id, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            IFormFile file;
            var movie = dBContext.Movies.Find(id);
            if (movie != null)
            {
                var genres = dBContext.Genres.ToList();
                ViewData["Genres"] = genres;
                return View(movie);
            }
            ViewData["message"] = $"MovieID: {id} không tồn tại.";
            return View("Error");
        }
        public IActionResult Comments()
        {
            if (HttpContext.Session.GetString("account") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Person p = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (p.Type == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.rates = dBContext.Genres.ToList();
            return View();
        }
    }
}
