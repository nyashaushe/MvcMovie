using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MvcMovie.Data;
using System;
using System.Linq;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? year)
        {
            // Seed database if empty
            if (!_context.Movie.Any())
            {
                _context.Movie.AddRange(
                    new Movie { Title = "Inception", ReleaseDate = new DateTime(2010, 7, 16), Genre = "Sci-Fi", Price = 9.99M },
                    new Movie { Title = "The Lion King", ReleaseDate = new DateTime(1994, 6, 24), Genre = "Animation", Price = 7.99M },
                    new Movie { Title = "Interstellar", ReleaseDate = new DateTime(2014, 11, 7), Genre = "Sci-Fi", Price = 10.99M }
                );
                _context.SaveChanges();
            }

            var movies = _context.Movie.AsQueryable();
            if (year.HasValue)
            {
                movies = movies.Where(m => m.ReleaseDate.Year >= year.Value);
            }
            var movieList = movies.ToList();
            ViewData["SelectedYear"] = year;
            ViewData["Years"] = _context.Movie.Select(m => m.ReleaseDate.Year).Distinct().OrderByDescending(y => y).ToList();
            return View(movieList);
        }
    }
}
