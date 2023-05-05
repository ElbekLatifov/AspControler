namespace AspControler.Models;

public class User
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? UserPhoto { get; set; }
    public int CurrentTicketIndex {get;set;}
    // public List<Urinishlar> Attemps { get; set; }
    // public List<TicketResult> Results { get; set; }
    // public List<TicketsInfo> TicketInfo { get; set; }
}