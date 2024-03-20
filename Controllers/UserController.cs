using CRMSystem.Data;
using CRMSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;


namespace CRMSystem.Controllers
{
    public class UserController : Controller
    {
        // Field to hold an instance of the ApplicationDbContext.
        private readonly ApplicationDbContext _context;

        // Constructor for UserController, initializing the _context field with the ApplicationDbContext injected by the dependency injection system.
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User/Login
        // Action method for handling GET requests for the User Login page.
        // Checks if the user is already authenticated and redirects to the home page if so.
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            // Check if the user is already authenticated. If yes, redirect to the home page.
            if(claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
 
            // Return the Login view if the user is not authenticated.
            return View();
        }

    // Action method for handling POST requests for User Login.
    // This method is where the authentication logic is implemented.
    [HttpPost]
    public async Task<IActionResult> Login(User modelLogin)
    {
        // Dummy check for username and password
        // In a real application, you should validate against a database or other data source.
        if ((modelLogin.Username == "manager" && modelLogin.Password == "123") || 
            (modelLogin.Username == "employee" && modelLogin.Password == "123"))
        {
            // Determine the role
            string role = modelLogin.Username == "manager" ? "Manager" : "Employee";

            // Set up claims
            List<Claim> claims = new List<Claim>(){
                new Claim(ClaimTypes.NameIdentifier, modelLogin.Username),
                new Claim(ClaimTypes.Role, role)
            };

            // Create claims identity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Authentication properties
            AuthenticationProperties properties = new AuthenticationProperties(){
                AllowRefresh = true
            };

            // Sign in
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            // Redirect to home page
            return RedirectToAction("Index", "Home");
        }

    // If the user is not found or credentials are invalid, show an error message.
    ViewData["ValidateMessage"] = "user not found";
    return View();
}
}
}