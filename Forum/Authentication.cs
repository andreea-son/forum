using Forum.Dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class Authentication
    {
       
        public static int AddUser(string? name, string? password, UserTypes userType)
        {
            ValidatePassword(name, password);
            if (GetUser(name) != null)
                throw new Exception("The username is taken!");
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("INSERT INTO users (username, password, is_admin, is_moderator) VALUES (@username, @password, @isAdmin, @isModerator);", connection);

            command.Parameters.AddWithValue("@username", name);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@isAdmin", userType == UserTypes.Admin);
            command.Parameters.AddWithValue("@isModerator", userType == UserTypes.Moderator);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }

        public static User? GetUser(string? name)
        {
            ValidateUsername(name);
            User? user = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM users WHERE username = @username;", connection);

            command.Parameters.AddWithValue("@username", name);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    user = new User()
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        UserType = reader.GetBoolean(3) ? UserTypes.Admin : reader.GetBoolean(4) ? UserTypes.Moderator : UserTypes.User
                    };
                }
            }
            connection.Close();
            return user;
        }

        public static User? GetUser(string? name, string? password) 
        {
            ValidatePassword(name, password);
            User? user = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM users WHERE username = @username AND password = @password;", connection);

            command.Parameters.AddWithValue("@username", name);
            command.Parameters.AddWithValue("@password", password);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    user = new User()
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        UserType = reader.GetBoolean(3) ? UserTypes.Admin : reader.GetBoolean(4) ? UserTypes.Moderator : UserTypes.User
                    };
                }
            }
            connection.Close();
            return user;
        }

        private static void ValidateUsername(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("The username is empty!");
        }

        private static void ValidatePassword(string? name, string? password)
        {
            ValidateUsername(name);
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("The password is empty!");
        }
    }
}
