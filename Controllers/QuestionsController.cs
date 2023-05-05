using System;
using System.Linq;
using AspControler.Models;
using AspControler.Repositories;
using AspControler.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspControler.Controllers;

public class QuestionsController : Controller
{
    private readonly List<QuestionModel>? _questions;
    public QuestionsController()
    {
        _questions = QuestionsService._Salom._questions; 
    }
    
    public IActionResult Index()
    {
        ViewBag.Questions = _questions;

        return View();
    }

    public async Task<IActionResult> GetQuestionByIndex(int id, int? ticketIndex = null, int? choiseIndex = null, bool restart=false,bool  attemps = false)
    {
        var userid = HttpContext.Request.Cookies["UserId"];
        //var baza = new UserRepository();
        var user = UserService._userRepository.GetUserById(userid);
        var bilets = UserService._ticketRepository.GetUrinishs(userid);
        var bilet = bilets.FirstOrDefault(u=>u.TicketIndex == ticketIndex);
        if(attemps == true && restart == false)
        {
            int son = 0;
            var corr = 0;
            if(bilet.Attempts == 0)
            {  
                var ur = new Urinishlar()
                {
                    UserId = userid,
                    TicketIndex = ticketIndex.Value,
                    Attempts = 1
                };
                UserService._ticketRepository.UpdateUrinish(ur);
            } 
            var info = UserService._ticketRepository.GetTicketsInfos(userid);
            var inf = info.FindAll(u=>u.TicketIndex == ticketIndex);
            if(inf != null)

            for (int i = 0; i < inf.Count; i++)
            {
                son ++;
                if(inf[i].IsCorrect) corr ++;
            }
            if(son == 10)
            {
                son = 0; corr = 0;
                if (HttpContext.Request.Cookies.ContainsKey("Counter"))
                HttpContext.Response.Cookies.Delete("Counter");
                HttpContext.Response.Cookies.Append("Counter", son.ToString());
                if (HttpContext.Request.Cookies.ContainsKey("CurrentAnswersCount"))
                HttpContext.Response.Cookies.Delete("CurrentAnswersCount");
                HttpContext.Response.Cookies.Append("CurrentAnswersCount", corr.ToString());
            }else
            {
                if (HttpContext.Request.Cookies.ContainsKey("Counter"))
                    HttpContext.Response.Cookies.Delete("Counter");
                    HttpContext.Response.Cookies.Append("Counter", son.ToString());
                if (HttpContext.Request.Cookies.ContainsKey("CurrentAnswersCount"))
                    HttpContext.Response.Cookies.Delete("CurrentAnswersCount");
                    HttpContext.Response.Cookies.Append("CurrentAnswersCount", corr.ToString());
            }

        }
        else
            if(attemps == true && restart == true)
            {
                var san = bilet.Attempts;
                san ++;
                var e = new Urinishlar()
                {
                    UserId = userid,
                    TicketIndex = ticketIndex.Value,
                    Attempts = san
                };
                UserService._ticketRepository.UpdateUrinish(e);
            }
    
        if(restart == true)
        {
            var resulttt = new TicketResult()
            {
                UserId = userid,
                TicketIndex = ticketIndex!.Value,
                CorrectCount = 0,
                Vaqti = default
            };
            UserService._ticketRepository.UpdateResult(resulttt);

            UserService._ticketRepository.DeleteTicketInfo(userid, ticketIndex.Value);
        }

        var question = _questions?.FirstOrDefault(c => c.Id == id);

        if (ticketIndex != null)
        {
            HttpContext.Response.Cookies.Append("CurrentTicketIndex", $"{ticketIndex}");
            ViewBag.Ticketnumber = ticketIndex;
        }
        else
        {
            ViewBag.Ticketnumber = id/10;
        }

        if (HttpContext.Request.Cookies.ContainsKey("CurrentTicketIndex"))
        { 
            var index = Convert.ToInt32(HttpContext.Request.Cookies["CurrentTicketIndex"]);
                var indexx = Convert.ToInt32(HttpContext.Request.Cookies["Counter"]);
            if(indexx==10)
            {
                var correctcount = Convert.ToInt32(HttpContext.Request.Cookies["CurrentAnswersCount"]);

                var resultt = new TicketResult()
                {
                    UserId = userid,
                    CorrectCount = correctcount,
                    TicketIndex = index,
                    Vaqti = DateTime.Now
                };
                UserService._ticketRepository.UpdateResult(resultt);
                UserService._ticketRepository.AddHistory(resultt);
                
                HttpContext.Response.Cookies.Delete("CurrentTicketIndex");
                HttpContext.Response.Cookies.Delete("CurrentAnswersCount");
                HttpContext.Response.Cookies.Delete("Counter");
                return RedirectToAction(nameof(Result), new {TicketIndex = index, Correctcount = correctcount});
            }
        }

        if (question == null)
        {
            ViewBag.QuestionId = id;
            ViewBag.IsSuccess = false;
        }
        else
        {
            ViewBag.Question = question;
            ViewBag.IsSuccess = true;

            ViewBag.IsAnswer = choiseIndex != null;

            if (choiseIndex != null)
            {
                var infoos = new TicketsInfo()
                {
                    UserId = userid,
                    TicketIndex = ticketIndex.Value,
                    ChoisedIndex = choiseIndex,
                    QuestionIndex = question.Id,
                    IsAnswer = choiseIndex != null,
                    IsCorrect = question.Choices[choiseIndex.Value].Answer
                };
                UserService._ticketRepository.AddTicketInfo(infoos);

                if (HttpContext.Request.Cookies.ContainsKey("Counter"))
                {
                    var index = Convert.ToInt32(HttpContext.Request.Cookies["Counter"]);
                    HttpContext.Response.Cookies.Append("Counter", $"{index + 1}");
                }

                var answer = question!.Choices![(int)choiseIndex].Answer;
                ViewBag.ChoiseIndex = choiseIndex;
                ViewBag.Javob = answer;

                if (answer)
                {
                    if (HttpContext.Request.Cookies.ContainsKey("CurrentAnswersCount"))
                    {
                        var index = Convert.ToInt32(HttpContext.Request.Cookies["CurrentAnswersCount"]);
                        HttpContext.Response.Cookies.Append("CurrentAnswersCount", $"{index + 1}");
                    }
                }
            }
        }
        return View();
    }

    public IActionResult Result(int TicketIndex, int Correctcount)
    {
        ViewBag.TicketIndex = TicketIndex;
        ViewBag.Correctcount = Correctcount;
        return View();
    }
}
