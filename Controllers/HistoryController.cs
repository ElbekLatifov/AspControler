using System.ComponentModel.DataAnnotations;
using AspControler.Models;
using Microsoft.AspNetCore.Mvc;
using AspControler.Services;
using AspControler.Repositories;

namespace AspControler.Controllers;

public class HistoryController: Controller
{

    public IActionResult History()
    {
        return View();
    }  

    public IActionResult Clear()
    {
        var ismm = HttpContext.Request.Cookies["UserId"];
        UserService._ticketRepository.ClearHistory(ismm!);
        return RedirectToAction("History");
    }
}