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
    public class CustomersController : ApiController
    {
        // instead of using models directly, use dtos instead
        ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: api/Customers
        public IEnumerable<CustomerDto> Get(string query = null)
        {
            var q = _context.Customers.Include(c => c.MembershipType);

            if (!string.IsNullOrWhiteSpace(query))
                q = q.Where(c => c.Name.Contains(query));
            
            return q.ToList().Select(Mapper.Map<Customer, CustomerDto>);
            //return _context.Customers.Include(c => c.MembershipType).ToList().Select(Mapper.Map<Customer, CustomerDto>);
        }

        // GET: api/Customers/5
        //public CustomerDto Get(int id)
        public IHttpActionResult Get(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer is null)
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();

            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }

        // POST: api/Customers
        //public CustomerDto Post([FromBody]CustomerDto customerDto)
        public IHttpActionResult Post([FromBody]CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                return BadRequest();

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;

            //return customerDto;
            return Created(new Uri(Request.RequestUri + customer.Id.ToString()), customerDto);
        }

        // PUT: api/Customers/5
        //public void Put(int id, [FromBody]CustomerDto customerDto)
        public IHttpActionResult Put(int id, [FromBody]CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                return BadRequest();

            var existing = _context.Customers.Find(id);
            if (existing is null)
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();

            //existing.Name = customer.Name;
            //existing.Birthdate = customer.Birthdate;
            //existing.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            //existing.MembershipTypeId = customer.MembershipTypeId;
            Mapper.Map(customerDto, existing);

            _context.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Customers/5
        //public void Delete(int id)
        public IHttpActionResult Delete(int id)
        {
            var existing = _context.Customers.Find(id);
            if (existing is null)
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();

            _context.Customers.Remove(existing);
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
