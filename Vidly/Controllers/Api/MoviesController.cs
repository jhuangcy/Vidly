using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly2.DTOs;
using Vidly2.Models;

namespace Vidly2.Controllers.Api
{
    public class MoviesController : ApiController
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

        // GET: api/Movies
        public IEnumerable<MovieDto> Get(string query = null)
        {
            var q = _context.Movies.Include(m => m.Genre).Where(m => m.NumberAvailable > 0);

            if (!string.IsNullOrWhiteSpace(query))
                q = q.Where(m => m.Name.Contains(query));

            return q.ToList().Select(Mapper.Map<Movie, MovieDto>);
            //return _context.Movies.Include(m => m.Genre).ToList().Select(Mapper.Map<Movie, MovieDto>);
        }

        // GET: api/Movies/5
        public IHttpActionResult Get(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie is null)
                return NotFound();

            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }

        // POST: api/Movies
        [Authorize(Roles = RoleNames.CanManageMovies)]
        public IHttpActionResult Post([FromBody]MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = Mapper.Map<MovieDto, Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;

            return Created(new Uri(Request.RequestUri + movie.Id.ToString()), movieDto);
        }

        // PUT: api/Movies/5
        [Authorize(Roles = RoleNames.CanManageMovies)]
        public IHttpActionResult Put(int id, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var existing = _context.Movies.Find(id);
            if (existing is null)
                return NotFound();

            Mapper.Map(movieDto, existing);

            _context.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Movies/5
        [Authorize(Roles = RoleNames.CanManageMovies)]
        public IHttpActionResult Delete(int id)
        {
            var existing = _context.Movies.Find(id);
            if (existing is null)
                return NotFound();

            _context.Movies.Remove(existing);
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
