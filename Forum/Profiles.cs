using Forum.Dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class Profiles
    {
        public static int AddProfile(int userId)
        {
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("INSERT INTO profiles (user_id) VALUES (@userId);", connection);
            command.Parameters.AddWithValue("@userId", userId);
            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }

        public static Profile? GetProfile(int id)
        {
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT p.id, p.user_id, p.bio, p.profile_photo, p.name, p.age, p.location, p.gender, u.username " +
                "FROM profiles AS p " +
                "LEFT JOIN Users AS u ON p.user_id = u.id " +
                "WHERE p.user_id = @userId;", connection);

            command.Parameters.AddWithValue("@userId", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new Profile()
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        Bio = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        ProfilePhoto = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                        Name = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        Age = reader.GetInt32(5),
                        Location = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                        Gender = !reader.IsDBNull(7) ? reader.GetString(7) : null,
                        Username = reader.GetString(8)
                    };
                }
            }
            connection.Close();
            return null;
        }

        public static int UpdateProfile(int userId, string? bio, string? profilePhoto, string? name, int age, string? location, string? gender)
        {
            ValidateAge(age);
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("UPDATE profiles SET bio = @bio, profile_photo = @profilePhoto, name = @name, age = @age, location = @location, gender = @gender " +
                "WHERE user_id = @userId;", connection);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@bio", bio);
            command.Parameters.AddWithValue("@profilePhoto", profilePhoto);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@age", age);
            command.Parameters.AddWithValue("@location", location);
            command.Parameters.AddWithValue("@gender", gender);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }

        private static void ValidateAge(int age)
        {
            if (age < 0)
                throw new ArgumentException("Age cannot be negative");
        }
    }
}
