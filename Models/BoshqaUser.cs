namespace AspControler.Models;

public class BoshqaUser
{
    public string Id {get;set;}
    public List<Urinishlar> Attemps { get; set; }
    public List<TicketResult> Results { get; set; }
    public List<TicketsInfo> TicketInfo { get; set; }


}