using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using Vidly2.Models;
using Vidly2.ViewModels;

namespace Vidly2.Controllers
{
    public class CustomersController : Controller
    {
        //List<Customer> customers = new List<Customer>()
        //{
        //    new Customer() { Id = 1, Name = "John Smith" },
        //    new Customer() { Id = 2, Name = "Marry Williams" },
        //};

        ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers
        public ActionResult Index()
        {
            // data caching: should only be done if you know action has poor performance
            if (MemoryCache.Default["Genres"] == null)
                MemoryCache.Default["Genres"] = _context.Genres.ToList();

            var genres = MemoryCache.Default["Genres"] as IEnumerable<Genre>;

            //var customers = _context.Customers.Include(c => c.MembershipType).ToList();
            //return View(customers);

            // customer data will be retrieved by the datatable though an api instead
            return View();
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            //var customer = customers.FirstOrDefault(c => c.Id == id);
            var customer = _context.Customers.Include(c => c.MembershipType).FirstOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();
            
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var vm = new CustomerFormVM() 
            {
                Customer = new Customer(),  // this is to not show the validation error for id in the validation summary
                MembershipTypes = membershipTypes 
            };

            return View("CustomerForm", vm);
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                // TODO: Add insert logic here
                _context.Customers.Add(customer);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();

            var vm = new CustomerFormVM()
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            return View("CustomerForm", vm);
        }

        // POST: Customers/Edit/5
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
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var vm = new CustomerFormVM()
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("CustomerForm", vm);
            }

            try
            {
                // TODO: Add insert/update logic here
                if (customer.Id == 0)
                {
                    _context.Customers.Add(customer);
                }
                else
                {
                    var existing = _context.Customers.Find(customer.Id);

                    //TryUpdateModel(existing);   // not very safe
                    existing.Name = customer.Name;
                    existing.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                    existing.Birthdate = customer.Birthdate;
                    existing.MembershipTypeId = customer.MembershipTypeId;
                }

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customers/Delete/5
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
