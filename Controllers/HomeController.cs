using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspControler.Models;

namespace AspControler.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Tickets()
    {
        return View();
    }


    public IActionResult Privacy(int a, int b)
    {
        
        return View();
    }
    public IActionResult Ogohlantirish()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
