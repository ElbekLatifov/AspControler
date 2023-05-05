using AspControler.Repositories;
using AspControler.Models;

namespace AspControler.Services;

public static class UserService
{
    public static UserRepository _userRepository = new UserRepository();
    public static TicketRepository _ticketRepository = new TicketRepository();

    public static bool IsLoggedIn(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey("UserId")) return false;
        
        var userId = context.Request.Cookies["UserId"];
      
        var user = _userRepository.GetUserById(userId);

        return user != null;
    }

}