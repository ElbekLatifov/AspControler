using AspControler.Models;
using Microsoft.Data.Sqlite;

namespace AspControler.Repositories;

public class UserRepository
{
    private readonly SqliteConnection _connection;

    public UserRepository()
    {
        _connection = new SqliteConnection("Data Source=data.db;");
        _connection.Open();
        
        CreateUserTable();
    }   

    public void CreateUserTable()
    {
        var _command = _connection.CreateCommand();
        _command.CommandText = "CREATE TABLE IF NOT EXISTS Copyuser(user_id TEXT UNIQUE, username TEXT NOT NULL, password TEXT, email TEXT, phone TEXT, photourl TEXT, current_ticket_index INTEGER DEFAULT 0)";
        _command.ExecuteNonQuery();
    }

    public void AddUser(User user)
    {
        var _command = _connection.CreateCommand();
        _command.CommandText = $"INSERT INTO Copyuser(user_id, username, password, email, phone, photourl) VALUES(@id, @name, @password, @email, @phone, @url)";
        _command.Parameters.AddWithValue("id", user.Id);
        _command.Parameters.AddWithValue("name", user.UserName);
        _command.Parameters.AddWithValue("password", user.Password);
        _command.Parameters.AddWithValue("email", user.Email);
        _command.Parameters.AddWithValue("phone", user.Phone);
        _command.Parameters.AddWithValue("url", user.UserPhoto);
        _command.Prepare();
        _command.ExecuteNonQuery();
    }

    public void UpdateUser(User user)
    {
        var _command = _connection.CreateCommand();
        _command.CommandText = $"UPDATE Copyuser SET username = @name, password = @password, email = @email, phone = @phone, photourl = @url WHERE user_id = @id";
        _command.Parameters.AddWithValue("id", user.Id);
        _command.Parameters.AddWithValue("name", user.UserName);
        _command.Parameters.AddWithValue("password", user.Password);
        _command.Parameters.AddWithValue("email", user.Email);
        _command.Parameters.AddWithValue("phone", user.Phone);
        _command.Parameters.AddWithValue("url", user.UserPhoto);
        _command.Prepare();
        _command.ExecuteNonQuery();
    }

    public void DeleteUser(User user)
    {
        var _command = _connection.CreateCommand();
        _command.CommandText = $"DELETE FROM Copyuser WHERE user_id = @id";
        _command.Parameters.AddWithValue("id", user.Id);
        _command.Prepare();
        _command.ExecuteNonQuery();
    }

    public User? GetUserById(string id)
    {
        var _command = _connection.CreateCommand();
        _command.CommandText = $"SELECT * FROM Copyuser WHERE user_id = @id";
        _command.Parameters.AddWithValue("id", id);
        _command.Prepare();
        var reader = _command.ExecuteReader();

        while(reader.Read())
        {
            var user = new User()
            {
                Id = reader.GetString(0),
                UserName = reader.GetString(1),
                Password = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4),
                UserPhoto = reader.GetString(5),
                CurrentTicketIndex = reader.GetInt32(6)
            };

            reader.Close();
            return user;
        }

        reader.Close();
        return null;
    }

    public List<User>? GetUsers()
    {
        var _command = _connection.CreateCommand();
        var users = new List<User>();
        _command.CommandText = "SELECT * FROM Copyuser;";
        var reader = _command.ExecuteReader();
        while(reader.Read())
        {
            var user = new User()
            {
                Id = reader.GetString(0),
                UserName = reader.GetString(1),
                Password = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4),
                UserPhoto = reader.GetString(5)
            };

            users.Add(user);
        }
        reader.Close();
        return users;
    }
}