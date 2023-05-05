namespace AspControler.Models;

public class TicketsInfo
{
    public string UserId {get; set;}
    public int TicketIndex { get; set; }
    public int? ChoisedIndex { get; set; }
    public bool IsAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionIndex { get; set; }
}