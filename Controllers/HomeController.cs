using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRM_system.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace CRM_system.Controllers;

// The Authorize attribute enforces that all actions in this controller require authentication.
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Action method for the home page.
    // This method handles HTTP GET requests to the root URL and returns the Index view.
    public IActionResult Index()
    {
        return View();
    }

    // Action method for the Privacy page.
    // This method returns the Privacy view.
    public IActionResult Privacy()
    {
        return View();
    }

    // Action method for logging out.
    // This asynchronous method signs the user out and redirects to the Login page.
    public async Task<IActionResult> LogOut()
    {
        // Sign out using cookie authentication scheme.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        // Redirect to the Login action in the User controller.
        return RedirectToAction("Login", "User");
    }

    // Action method for handling errors.
    // This method returns the Error view with error information.
    // The ResponseCache attribute specifies not to cache the response to ensure error information is always up to date.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // Create an ErrorViewModel instance with the current request ID.
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
