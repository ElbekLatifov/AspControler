using Microsoft.AspNetCore.Mvc;
using AspControler.Models;
using Newtonsoft.Json;
using AspControler.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AspControler.Controllers;


public class TicketsController : Controller
{
    private List<QuestionModel> __questions;
    public TicketsController()
    {
        var yul = Path.Combine("JsonData", "uzlotin.json");
        var json = System.IO.File.ReadAllText(yul);

        __questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json)!;
    }
    public IActionResult Tickets(int page=0)
    {
        ViewBag.IdPage = page;
        ViewBag.TicketsCount = __questions.Count/10;
        return View();
    }

    public IActionResult Search(int ticketNumber)
    {
        if(ticketNumber>0 && ticketNumber<=70)
        {
            ViewBag.Number = ticketNumber;
            ViewBag.TicketsSoni = __questions.Count/10;
            return View();
        }
        
        return RedirectToAction("NotFound", "Tickets");
    }

    public IActionResult NotFound()
    {
        return View();
    }
}