using CRMSystem.Data;
using CRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSystem.Controllers
{
    // This attribute restricts access to the controller to users with 'Employee' or 'Manager' roles.
     [Authorize(Roles = "Employee, Manager")]
    public class CallController : Controller
    {
        // A private field to hold the application's database context.
        private readonly ApplicationDbContext _context;


        // Constructor for the CallController. It initializes the context field.
        public CallController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Call
        // An asynchronous action method to handle HTTP GET requests for the Call index page.
        // It optionally filters calls by customer number.
        public async Task<IActionResult> Index(int? customerNo)
        {
            // If a customer number is provided, filter calls for that customer; otherwise, return all calls.
            if (customerNo.HasValue)
            {
                return View(await _context.Calls.Where(c => c.CustomerNo == customerNo.Value).ToListAsync());
            }
            return View(await _context.Calls.ToListAsync());
        }

        // GET: Call/Create
        // Action method to handle GET requests for the Call creation page.
        // It populates ViewData with a list of customers for a dropdown in the view.
        public IActionResult Create()
        {
            ViewData["CustomerNo"] = new SelectList(_context.Customers, "CustomerNo", "Name");
            return View();
        }

        // POST: Call/Create
        // An asynchronous action method to handle POST requests for creating a Call.
        // [ValidateAntiForgeryToken] protects against Cross-Site Request Forgery (CSRF) attacks.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CallNo,CustomerNo,DateOfCall,TimeOfCall,Subject")] Call call)
        {
            if (ModelState.IsValid)
            {
                _context.Add(call);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(call);
        }

        // GET: Call/Edit/5
        // An asynchronous action method to handle GET requests for editing a Call.
        // The id parameter specifies which call to edit.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await _context.Calls.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }
            return View(call);
        }

        // POST: Call/Edit/5
        // An asynchronous action method to handle POST requests for editing a Call.
        // It checks whether the call exists and handles concurrency issues.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CallNo,CustomerNo,DateOfCall,TimeOfCall,Subject")] Call call)
        {
            if (id != call.CallNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(call);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallExists(call.CallNo))
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
            return View(call);
        }

        // GET: Call/Delete/5
        // An asynchronous action method to handle GET requests for deleting a Call.
        // The id parameter specifies which call to delete.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await _context.Calls
                .FirstOrDefaultAsync(m => m.CallNo == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }
        
        // POST: Call/Delete/5
        // An asynchronous action method to handle POST requests for deleting a Call.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var call = await _context.Calls.FindAsync(id);
            if (call == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Calls.Remove(call);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // A private helper method to check if a Call exists in the database.
        private bool CallExists(int id)
        {
            return _context.Calls.Any(e => e.CallNo == id);
        }
    }
}
