using Forum.Dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class Searching
    {
        public static List<Search> SearchForum(string term)
        {
            ValidateSearchTerm(term);
            List<Search> results = new();
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT id, name FROM categories WHERE name COLLATE utf8mb4_general_ci LIKE CONCAT('%', @term, '%') OR description COLLATE utf8mb4_general_ci LIKE CONCAT('%', @term, '%');", connection);
            command.Parameters.AddWithValue("@term", term);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(new Search()
                    {
                        Id = reader.GetInt32(0),
                        Result = reader.GetString(1),
                        IsCategory = true
                    });
                }
            }
            command = new("SELECT id, title FROM subjects WHERE title COLLATE utf8mb4_general_ci LIKE CONCAT('%', @term, '%') OR message COLLATE utf8mb4_general_ci LIKE CONCAT('%', @term, '%');", connection);
            command.Parameters.AddWithValue("@term", term);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(new Search()
                    {
                        Id = reader.GetInt32(0),
                        Result = reader.GetString(1),
                        IsSubject = true
                    });
                }
            }
            command = new("SELECT id, message FROM replies WHERE message COLLATE utf8mb4_general_ci LIKE CONCAT('%', @term, '%');", connection);
            command.Parameters.AddWithValue("@term", term);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(new Search()
                    {
                        Id = reader.GetInt32(0),
                        Result = reader.GetString(1),
                        IsReply = true
                    });
                }
            }
            connection.Close();
            return results;
        }

        private static void ValidateSearchTerm(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                throw new ArgumentException("Search term cannot be empty!");
        }
    }
}
