using AspControler.Models;
using Microsoft.Data.Sqlite;

namespace AspControler.Repositories;

public class TicketRepository
{
    private readonly SqliteConnection _connection;

    public TicketRepository()
    {
        _connection = new SqliteConnection("Data Source=data.db;");
        _connection.Open();
        
        CreateUserTable();
    }   

    public void CreateUserTable()
    {
        var _command = _connection.CreateCommand();
        _command.CommandText = "CREATE TABLE IF NOT EXISTS attemps(userid TEXT, ticketindex INTEGER, attemps INTEGER)";
        _command.ExecuteNonQuery();
        _command.CommandText = "CREATE TABLE IF NOT EXISTS ticketinfo(userid TEXT, ticket_id INTEGER, choised_id INTEGER, isanswer BOOLEAN, iscorrect BOOLEAN, question_id INTEGER)";
        _command.ExecuteNonQuery();
        _command.CommandText = "CREATE TABLE IF NOT EXISTS corrects(userid TEXT, ticketindex INTEGER, correct_count INTEGER, vaqt BIGINT)";
        _command.ExecuteNonQuery();
        _command.CommandText = "CREATE TABLE IF NOT EXISTS history(userid TEXT, ticketid INTEGER, corrects INTEGER, vaqt BIGINT)";
        _command.ExecuteNonQuery();
    }

    public void ClearHistory(string id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM history WHERE userid = @id";
        command.Parameters.AddWithValue("id", id);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void AddHistory(TicketResult qiymat)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO history(userid, ticketid, corrects, vaqt) VALUES(@userid, @id, @corrects, @time)";
        command.Parameters.AddWithValue("userid", qiymat.UserId);
        command.Parameters.AddWithValue("id", qiymat.TicketIndex);
        command.Parameters.AddWithValue("corrects", qiymat.CorrectCount);
        command.Parameters.AddWithValue("time", qiymat.Vaqti.Ticks);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public List<TicketResult> GetHistory(string name)
    {
        var ticketResult = new List<TicketResult>();
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM history WHERE userid = @name";
        command.Parameters.AddWithValue("name", name);
        command.Prepare();
        var reader = command.ExecuteReader();
        while(reader.Read())
        {
           ticketResult.Add(new TicketResult()
           {
                UserId = reader.GetString(0),
                TicketIndex = reader.GetInt32(1),
                CorrectCount = reader.GetInt32(2),
                Vaqti = DateTime.FromFileTime(reader.GetInt64(2))
           });
       }
       reader.Close();
       return ticketResult;
}

    public void AddAttemp(Urinishlar qiymat)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO attemps(userid, ticketindex, attemps) VALUES(@userid, @id, @attemp)";
        command.Parameters.AddWithValue("userid", qiymat.UserId);
        command.Parameters.AddWithValue("id", qiymat.TicketIndex);
        command.Parameters.AddWithValue("attemp", qiymat.Attempts);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void AddTicketInfo(TicketsInfo qiymat)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO ticketinfo(userid, ticket_id, choised_id, isanswer, iscorrect, question_id)"+
         $"VALUES(@userid, @ticket_id, @choise_id, @isanswer, @iscorrect, @question_id)";
        command.Parameters.AddWithValue("userid", qiymat.UserId);
        command.Parameters.AddWithValue("ticket_id", qiymat.TicketIndex);
        command.Parameters.AddWithValue("choise_id", qiymat.ChoisedIndex);
        command.Parameters.AddWithValue("isanswer", qiymat.IsAnswer);
        command.Parameters.AddWithValue("iscorrect", qiymat.IsCorrect);
        command.Parameters.AddWithValue("question_id", qiymat.QuestionIndex);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void AddResult(TicketResult qiymat)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO corrects(userid, ticketindex, correct_count, vaqt) VALUES(@userid, @id, @corrects, @time)";
        command.Parameters.AddWithValue("userid", qiymat.UserId);
        command.Parameters.AddWithValue("id", qiymat.TicketIndex);
        command.Parameters.AddWithValue("corrects", qiymat.CorrectCount);
        command.Parameters.AddWithValue("time", qiymat.Vaqti.Ticks);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void UpdateUrinish(Urinishlar ur)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "UPDATE attemps SET attemps = @urinish WHERE userid = @indexx AND ticketindex = @index";
        command.Parameters.AddWithValue("indexx", ur.UserId);
        command.Parameters.AddWithValue("index", ur.TicketIndex);
        command.Parameters.AddWithValue("urinish", ur.Attempts);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void UpdateResult(TicketResult ticket)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "UPDATE corrects SET correct_count = @count, vaqt = @time WHERE userid = @index AND ticketindex = @indexx";
        command.Parameters.AddWithValue("index", ticket.UserId);
        command.Parameters.AddWithValue("indexx", ticket.TicketIndex);
        command.Parameters.AddWithValue("count", ticket.CorrectCount);
        command.Parameters.AddWithValue("time", ticket.Vaqti.Ticks); 
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void UpdateTicketInfo(TicketsInfo info)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "UPDATE ticketinfo SET choised_id = @choise, isanswer = @answer, iscorrect = @correct, question_id = @qid WHERE userid = @indexx AND ticket_id = @index";
        command.Parameters.AddWithValue("indexx", info.UserId);
        command.Parameters.AddWithValue("index", info.TicketIndex);
        command.Parameters.AddWithValue("choise", info.ChoisedIndex);
        command.Parameters.AddWithValue("correct", info.IsCorrect);
        command.Parameters.AddWithValue("answer", info.IsAnswer);
        command.Parameters.AddWithValue("qid", info.QuestionIndex);
        command.Prepare();
        command.ExecuteNonQuery();
    }

    public void DeleteTicketInfo(string id, int index)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM ticketinfo WHERE userid = @id AND ticket_id = @index";
        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("index", index);
        command.Prepare();
        command.ExecuteNonQuery();
    }
    // public Urinishlar? GetUrinish(int user_id)
    // {
        // var command = _connection.CreateCommand();
        // command.CommandText = "SELECT * FROM attemps WHERE ticketindex = @user_id";
        // command.Parameters.AddWithValue("user_id", user_id);
        // command.Prepare();
        // var reader = command.ExecuteReader();
        // while(reader.Read())
        // {
            // var urinish = new Urinishlar()
            // {
                // UserId = reader.GetString(0),
                // TicketIndex = reader.GetInt32(1),
                // Attempts = reader.GetInt32(2),
            // };

            // reader.Close();
            // return urinish;
        // }
        // reader.Close();
        // return null;
    // }

    // public TicketsInfo? GetTicketsInfo(int ticket_id)
    // {
        // var command = _connection.CreateCommand();
        // command.CommandText = "SELECT * FROM ticketinfo WHERE ticket_id = @user_id";
        // command.Parameters.AddWithValue("user_id", ticket_id);
        // command.Prepare();
        // var reader = command.ExecuteReader();
        // while(reader.Read())
        // {
            // var ticketInfo = new TicketsInfo()
            // {
                // TicketIndex = reader.GetInt32(0),
                // ChoisedIndex = reader.GetInt32(1),
                // IsAnswer = reader.GetBoolean(2),
                // IsCorrect = reader.GetBoolean(3),
                // QuestionIndex = reader.GetInt32(4)
            // };

            // reader.Close();
            // return ticketInfo;
        // }
        // reader.Close();
        // return null;
    // }

    // public TicketResult? GetResult(int ticket_id)
    // {
        // var command = _connection.CreateCommand();
        // command.CommandText = "SELECT * FROM corrects WHERE ticketindex = @ticket_id";
        // command.Parameters.AddWithValue("ticket_id", ticket_id);
        // command.Prepare();
        // var reader = command.ExecuteReader();
        // while(reader.Read())
        // {
            // var ticketResult = new TicketResult()
            // {
                // TicketIndex = reader.GetInt32(0),
                // CorrectCount = reader.GetInt32(1),
                // Vaqti = DateTime.FromFileTime(reader.GetInt64(2))
            // };

            // reader.Close();
            // return ticketResult;
        // }
        // reader.Close();
        // return null;
    // }


    public List<Urinishlar> GetUrinishs(string id)
    {
        var urinishlar = new List<Urinishlar>();
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM attemps WHERE userid = @id";
        command.Parameters.AddWithValue("id", id);
        command.Prepare();
        var reader = command.ExecuteReader();

        while(reader.Read())
        {
            urinishlar.Add(new Urinishlar()
            {
                UserId = reader.GetString(0),
                TicketIndex = reader.GetInt32(1),
                Attempts = reader.GetInt32(2)
            });
        }
        reader.Close();
        return urinishlar;
    }

    public List<TicketsInfo> GetTicketsInfos(string id)
    {
        var ticketsInfo = new List<TicketsInfo>();
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM ticketinfo WHERE userid = @id";
        command.Parameters.AddWithValue("id", id);
        command.Prepare();
        var reader = command.ExecuteReader();

        while(reader.Read())
        {
            ticketsInfo.Add(new TicketsInfo()
            {
                UserId = reader.GetString(0),
                TicketIndex = reader.GetInt32(1),
                ChoisedIndex = reader.GetInt32(2),
                IsAnswer = reader.GetBoolean(3),
                IsCorrect = reader.GetBoolean(4),
                QuestionIndex = reader.GetInt32(5)
            });
        }
        reader.Close();
        return ticketsInfo;
    }

    public List<TicketResult> GetResults(string id)
    {
        var ticketResult = new List<TicketResult>();
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM corrects WHERE userid = @id";
        command.Parameters.AddWithValue("id", id);
        command.Prepare();
       var reader = command.ExecuteReader();
       while(reader.Read())
       {
           ticketResult.Add(new TicketResult()
           {
                UserId = reader.GetString(0),
                TicketIndex = reader.GetInt32(1),
                CorrectCount = reader.GetInt32(2),
                Vaqti = DateTime.FromFileTime(reader.GetInt64(3))
           });
       }
       reader.Close();
       return ticketResult;
    }
}