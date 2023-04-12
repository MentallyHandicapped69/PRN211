using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjecPRN.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProjecPRN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CenimaDBContext _db;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = new CenimaDBContext();
        }

        public IActionResult Index()
        {
            ViewBag.gernes = _db.Genres.ToList();
            ViewBag.movies = _db.Movies.ToList();
            ViewBag.rates = _db.Rates.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        static string ConvertUnicode(string text)
        {
            text = text.Normalize(NormalizationForm.FormKD);
            Regex regex = new Regex("\\p{M}");
            return regex.Replace(text, string.Empty).ToLowerInvariant();
        }
        public IActionResult Search(string key, int cate)
        {
            ViewBag.gernes = _db.Genres.ToList();
            ViewBag.rates = _db.Rates.ToList();
            if (key != null)
            {
                List<Movie> movies = new List<Movie>();
                //ViewBag.movies = _db.Movies.Where(m => ConvertUnicode(m.Title).Contains(ConvertUnicode(key))).ToList();
                foreach (var item in _db.Movies.ToList())
                {
                    if (ConvertUnicode(item.Title.Trim()).Contains(ConvertUnicode(key.Trim())))
                    {
                        movies.Add(item);
                    }
                }
                ViewBag.movies = movies;
            }
            else if (cate != 0)
            {
                ViewBag.movies = _db.Movies.Include(c => c.Genre).Where(g => g.GenreId == cate).ToList();
            }
            else
            {
                ViewBag.movies = _db.Movies.ToList();
            }
            return View("Index", "Home");
        }

        public IActionResult Moviedetail(int id)
        {
            Movie m = _db.Movies.Include(c => c.Genre).Where(c => c.MovieId == id).FirstOrDefault();
            if (m != null)
            {
                ViewBag.m = m;
                ViewBag.rate = _db.Rates.Include(c => c.Person).Where(c => c.MovieId == id).ToList();
                return View();
            }
            ViewBag.Error = $"MovieId = {id} không tồn tại!!!!";
            return View("../Shared/Error");
        }


        [HttpPost]
        public IActionResult Rate(Rate r)
        {

            Person acc = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("account"));
            if (r != null)
            {
                ViewBag.m = _db.Movies.Include(c => c.Genre).Where(c => c.MovieId == r.MovieId).FirstOrDefault();
                ViewBag.rate = _db.Rates.Include(c => c.Person).Where(c => c.MovieId == r.MovieId).ToList();
                DateTime now = DateTime.Now;
                r.Time = now;


                var cmt = _db.Rates.Where(m => m.Movie.MovieId == r.MovieId && m.PersonId == acc.PersonId);
                _db.Rates.Add(r);

                _db.SaveChanges();
                return View("MovieDetail");

            }

            ViewBag.Error = $"ERROR";
            return View("../Shared/Error");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}