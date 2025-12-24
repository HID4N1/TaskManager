using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers;

/// <summary>
/// Home controller for landing page
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Project");
        }
        return RedirectToAction("Login", "Auth");
    }

    public IActionResult Error()
    {
        return View();
    }
}

