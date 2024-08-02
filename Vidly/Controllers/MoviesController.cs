using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly2.Models;
using Vidly2.ViewModels;

namespace Vidly2.Controllers
{
    public class MoviesController : Controller
    {
        ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>()
            {
                new Customer() { Name = "John Doe" },
                new Customer() { Name = "Sue Storm" },
            };
            var vm = new RandomMovieVM() { Movie = movie, Customers = customers };

            // other ways to pass data to view
            ViewData["Movie"] = movie;
            ViewBag.Movie = movie;

            //return View(movie);
            return View(vm);
            //return Content("Hello World");  // ignores any markup/code in the view file
            //return HttpNotFound();
            //return new EmptyResult();   // ignores any markup/code in the view file, displays nothing
            //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" });    // 3rd arg becomes url params
        }

        // using custom route /movies/released/2015/4
        // this is the default controller named in RouteConfig
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        // to use route attribs, enable in RouteConfig
        [Route("movies/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
        public ActionResult ByReleaseYear(int year, int month)
        {
            return Content(year + "/" + month);
        }

        // GET: Movies
        public ActionResult Index(int? pageIndex, string sortBy)    // strings are nullable by default
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;

            if (string.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            //var movies = new List<Movie>()
            //{
            //    new Movie() { Name = "Shrek" },
            //    new Movie() { Name = "Wall-e" },
            //};

            //var movies = _context.Movies.Include(m => m.Genre).ToList();
            //return View(movies);

            if (User.IsInRole(RoleNames.CanManageMovies))
                return View();
            
            return View("Index_Guest");
            //return Content($"pageIndex={pageIndex}&sortBy={sortBy}");
        }

        // GET: Movies/Details/5
        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).FirstOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = RoleNames.CanManageMovies)]
        public ActionResult Create()
        {
            var genres = _context.Genres.ToList();
            var vm = new MovieFormVM() 
            {
                //Movie = new Movie(),    // dont do this because it will put default values into datetime, int, etc
                Genres = genres 
            };

            return View("MovieForm", vm);
        }

        // POST: Movies/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = RoleNames.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return HttpNotFound();

            var vm = new MovieFormVM(movie)
            {
                //Movie = movie,

                // this can be done in the vm constructor
                //Id = movie.Id,
                //Name = movie.Name,
                //ReleaseDate = movie.ReleaseDate,
                //NumberInStock = movie.NumberInStock,
                //GenreId = movie.GenreId,

                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", vm);

            // also works if movies/edit?id=1
            // if the param name is changed (eg. movieId), then this route can only be accessed with movies/edit?movieId=1 due to the default param set in RouteConfig
            //return Content("id=" + id); 
        }

        // POST: Movies/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // this is used to handle create/edit submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.CanManageMovies)]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var vm = new MovieFormVM(movie)
                {
                    //Movie = movie,
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", vm);
            }

            try
            {
                // TODO: Add insert/update logic here
                if (movie.Id == 0)
                {
                    movie.DateAdded = DateTime.Now;
                    _context.Movies.Add(movie);
                }
                else
                {
                    var existing = _context.Movies.Find(movie.Id);

                    existing.Name = movie.Name;
                    existing.ReleaseDate = movie.ReleaseDate;
                    existing.NumberInStock = movie.NumberInStock;
                    existing.GenreId = movie.GenreId;
                }

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Movies/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
