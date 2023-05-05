using System.ComponentModel.DataAnnotations;
using AspControler.Models;
using Microsoft.AspNetCore.Mvc;
using AspControler.Services;
using AspControler.Repositories;

namespace AspControler.Controllers;

public class UsersController: Controller
{
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }  

    [HttpGet]
    public IActionResult Update()
    {
        return View();
    }  

    [HttpPost]
    public async Task<IActionResult> UpdatePost(CopyUser copyuser)
    {
        var ismm = HttpContext.Request.Cookies["UserId"];

        var user = new User()
        {
            Id = ismm,
            UserName = copyuser.UserName,
            Phone = copyuser.Phone,
            Password = copyuser.Password,
            Email = copyuser.Email,
            UserPhoto = SavePhoto(copyuser.UserPhotos!)
        };
        
        UserService._userRepository.UpdateUser(user);
        HttpContext.Response.Cookies.Delete("UserId");
        HttpContext.Response.Cookies.Delete("UserName");
        HttpContext.Response.Cookies.Delete("UserPassword");

        HttpContext.Response.Cookies.Append("UserId", user.Id);
        HttpContext.Response.Cookies.Append("UserName", user.UserName!);
        HttpContext.Response.Cookies.Append("UserPassword", user.Password!);

        return RedirectToAction("Yangilandi");
    }
    [HttpGet]
    public IActionResult Yangilandi()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Check()
    {
         return View();
    }
    [HttpPost]
    public IActionResult Check(SignInUserModel singinmodel)
    {
        if(HttpContext.Request.Cookies.ContainsKey("UserName") && HttpContext.Request.Cookies.ContainsKey("UserPassword"))
        {
            var username = HttpContext.Request.Cookies["UserName"];
            var userpas = HttpContext.Request.Cookies["UserPassword"];
            if(singinmodel.UserName == username && singinmodel.Password == userpas)
            {
                return RedirectToAction("Update"); 
            }
            else
            {
                return RedirectToAction("Check"); 
            }
        }
        
        return RedirectToAction( "Ogohlantirish",  "Home");
    }

    [RequireHttps]
    [HttpPost]
    public IActionResult SignIn(SignInUserModel singinmodel)
    {
        var users = UserService._userRepository.GetUsers();
        foreach(var item in users!)
        {
            if(singinmodel.UserName == item.UserName && singinmodel.Password == item.Password)
            {
                if(HttpContext.Request.Cookies.ContainsKey("Counter"))
                    HttpContext.Response.Cookies.Delete("Counter");
                if(HttpContext.Request.Cookies.ContainsKey("CurrentAnswersCount"))
                    HttpContext.Response.Cookies.Delete("CurrentAnswersCount");
                if(HttpContext.Request.Cookies.ContainsKey("CurrentTicketIndex"))
                    HttpContext.Response.Cookies.Delete("CurrentTicketIndex");
                
                HttpContext.Response.Cookies.Delete("UserId");
                HttpContext.Response.Cookies.Delete("UserName");
                HttpContext.Response.Cookies.Delete("UserPassword");
                HttpContext.Response.Cookies.Append("UserId", item.Id!);
                HttpContext.Response.Cookies.Append("UserName", item.UserName!);
                HttpContext.Response.Cookies.Append("UserPassword", item.Password!);
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                return RedirectToAction("SignIn"); 
            }
        }
        return RedirectToAction("SignUp");
    }
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignUp(CopyUser copyuser)
    {
        if(ModelState.IsValid)
        {
            return View(copyuser);
        }
        var user = new User()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = copyuser.UserName,
            Phone = copyuser.Phone,
            Password = copyuser.Password,
            Email = copyuser.Email,
            UserPhoto = SavePhoto(copyuser.UserPhotos!)
        };

       // var baza = new UserRepository();
       // baza.AddUser(user);
        for (int i = 0; i < 70; i++)
        {
            var urinish = new Urinishlar()
            {
                UserId = user.Id,
                TicketIndex = i,
                Attempts = 0
            };
            UserService._ticketRepository.AddAttemp(urinish);
        }

        for (int i = 0; i < 70; i++)
        {
            var result = new TicketResult()
            {
                UserId = user.Id,
                TicketIndex = i,
                CorrectCount = 0,
                Vaqti = default
            };
            UserService._ticketRepository.AddResult(result);
        }
        if(HttpContext.Request.Cookies.ContainsKey("Counter"))
            HttpContext.Response.Cookies.Delete("Counter");
        if(HttpContext.Request.Cookies.ContainsKey("CurrentAnswersCount"))
            HttpContext.Response.Cookies.Delete("CurrentAnswersCount");
        if(HttpContext.Request.Cookies.ContainsKey("CurrentTicketIndex"))
            HttpContext.Response.Cookies.Delete("CurrentTicketIndex");

        UserService._userRepository.AddUser(user);
        HttpContext.Response.Cookies.Append("UserId", user.Id);
        HttpContext.Response.Cookies.Append("UserName", user.UserName!);
        HttpContext.Response.Cookies.Append("UserPassword", user.Password!);

        return RedirectToAction("Index", "Home");
    }
    public IActionResult Profile()
    {
        if(HttpContext.Request.Cookies.ContainsKey("UserId"))
        {
            var userid = HttpContext.Request.Cookies["UserId"];
            //var baza = new UserRepository();
            //var user = baza.GetUserById(userid);
            var user = UserService._userRepository.GetUserById(userid);
            return View(user);

        }
        return RedirectToAction("SignUp", "Users");
    }

    private string SavePhoto(IFormFile file)
    {
        if(!Directory.Exists("wwwroot/UserImages"))
        {
            Directory.CreateDirectory("wwwroot/UserImages");
        }

        var fileName = Guid.NewGuid().ToString()+".jpg";

        var ms = new MemoryStream();
        file.CopyTo(ms);
        System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "UserImages",fileName), ms.ToArray());

        return "/UserImages/" + fileName;
    }

    public IActionResult SelectLanguage(string language)
    {
        QuestionsService._Salom.Language(language);
        return RedirectToAction("Tickets", "Tickets");
    }

    public async Task<IActionResult> LogOut()
    {
        if(HttpContext.Request.Cookies.ContainsKey("UserId"))
        {
            var userid = HttpContext.Request.Cookies["UserId"];
            var user = UserService._userRepository.GetUserById(userid);
           UserService._userRepository.DeleteUser(user);
           
            //var baza = new UserRepository();
            //var user = baza.GetUserById(userid);
            
            HttpContext.Response.Cookies.Delete("UserId");
            HttpContext.Response.Cookies.Delete("UserName");
            HttpContext.Response.Cookies.Delete("UserPassword");
            //var yn = new UserRepository();
            //yn.DeleteUser(user);
            //await UserService.Save();
            return RedirectToAction("Index", "Home");
        } else
        return RedirectToAction("Profile", "Users");

    }
}