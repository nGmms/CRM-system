using CRMSystem.Data;
using CRMSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace CRMSystem.Controllers
{
    // Authorize attribute restricts access to the controller actions to users with the 'Manager' role.
    [Authorize(Roles = "Manager")]
    public class CustomerController : Controller
    {
        // Field to hold an instance of the ApplicationDbContext.
        private readonly ApplicationDbContext _context;

        // Constructor that initializes the context field with the injected ApplicationDbContext.
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer
        // Action method for handling GET requests to the Customer index page.
        // Retrieves a list of all customers asynchronously and passes it to the view.
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customer/Create
        // Action method for handling GET requests to the Customer creation page.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // Action method for handling POST requests for Customer creation.
        // [ValidateAntiForgeryToken] is used to prevent Cross-Site Request Forgery attacks.
        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerNo,Name,Surname,Address,PostCode,Country,DateOfBirth")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        // Action method for handling GET requests to edit a specific Customer.
        // The id parameter indicates which customer to edit.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // Action method for handling POST requests to update a specific Customer.
        // Validates the model and handles concurrency exceptions.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerNo,Name,Surname,Address,PostCode,Country,DateOfBirth")] Customer customer)
        {
            if (id != customer.CustomerNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        // Action method for handling GET requests to delete a specific Customer.
        // The id parameter specifies the customer to delete.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerNo == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        // Action method for handling POST requests to confirm deletion of a Customer.
        // Deletes associated calls before deleting the customer.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            // Retrieve and delete all calls associated with the customer
            var calls = _context.Calls.Where(c => c.CustomerNo == id);
            if (calls.Any())
            {
                _context.Calls.RemoveRange(calls);
            }

            // Delete the customer
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // Helper method to check if a Customer exists in the database.
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerNo == id);
        }
    }
}