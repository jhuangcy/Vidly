using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly2.DTOs;
using Vidly2.Models;

namespace Vidly2.Controllers.Api
{
    public class RentalsController : ApiController
    {
        ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: api/Rentals
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Rentals/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Rentals
        public IHttpActionResult Post([FromBody]RentalDto rentalDto)
        {
            if (rentalDto.MovieIds.Count == 0)
                return BadRequest("No movies were provided.");

            var customer = _context.Customers.Find(rentalDto.CustomerId);
            if (customer == null)
                return BadRequest("Customer not found.");

            var movies = _context.Movies.Where(m => rentalDto.MovieIds.Contains(m.Id)).ToList();
            if (rentalDto.MovieIds.Count != movies.Count)
                return BadRequest("A movie was not found.");

            foreach (var m in movies)
            {
                if (m.NumberAvailable <= 0)
                    return BadRequest("Movie is not available.");

                m.NumberAvailable--;

                _context.Rentals.Add(new Rental() { DateRented = DateTime.Now, Customer = customer, Movie = m });
            }

            _context.SaveChanges();

            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Rentals/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Rentals/5
        public void Delete(int id)
        {
        }
    }
}
