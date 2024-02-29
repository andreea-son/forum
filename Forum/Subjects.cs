using Forum.Dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class Subjects
    {
        public static int AddSubject(string? title, string? message, int categoryId, int userId)
        {
            int topicId = 1;
            ValidateTitle(title);
            ValidateMessage(message);
            ValidateId(categoryId);
            ValidateId(userId);
            MySqlConnection connection = new(Settings.connString);
            connection.Open();

            MySqlCommand command1 = new("SELECT topic_id FROM subjects WHERE category_id = @categoryId ORDER BY topic_id DESC LIMIT 1;", connection);
            command1.Parameters.AddWithValue("@categoryId", categoryId);
            
            using (MySqlDataReader reader = command1.ExecuteReader())
            {
                if (reader.Read())
                {
                    topicId = reader.GetInt32(0) + 1;
                }
            }

            MySqlCommand command2 = new("INSERT INTO subjects (title, message, category_id, user_id, topic_id) VALUES (@title, @message, @categoryId, @userId, @topicId);", connection);

            command2.Parameters.AddWithValue("@title", title);
            command2.Parameters.AddWithValue("@message", message);
            command2.Parameters.AddWithValue("@categoryId", categoryId);
            command2.Parameters.AddWithValue("@userId", userId);
            command2.Parameters.AddWithValue("@topicId", topicId);

            int newRows = command2.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }

        public static List<Subject> GetByCategoryId(int categoryId)
        {
            ValidateId(categoryId);
            List<Subject> subjects = new();
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM subjects WHERE category_id = @categoryId;", connection);

            command.Parameters.AddWithValue("@categoryId", categoryId);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    subjects.Add(new Subject()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Message = reader.GetString(2),
                        CategoryId = reader.GetInt32(3),
                        UserId = reader.GetInt32(4),
                        Created = reader.GetDateTime(5),
                        TopicId = reader.GetInt32(6)
                    });
                }
            }
            connection.Close();
            return subjects;
        }

        public static Subject? GetSubject(int id)
        {
            ValidateId(id);
            Subject? subject = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM subjects WHERE id = @id;", connection);

            command.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    subject = new Subject()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Message = reader.GetString(2),
                        CategoryId = reader.GetInt32(3),
                        UserId = reader.GetInt32(4),
                        Created = reader.GetDateTime(5),
                        TopicId = reader.GetInt32(6)
                    };
                }
            }
            connection.Close();
            return subject;
        }

        public static List<Subject> GetSubjects()
        {
            List<Subject> subjects = new();
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM subjects;", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    subjects.Add(new Subject()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Message = reader.GetString(2),
                        CategoryId = reader.GetInt32(3),
                        UserId = reader.GetInt32(4),
                        Created = reader.GetDateTime(5),
                        TopicId = reader.GetInt32(6)
                    });
                }
            }
            connection.Close();
            return subjects;
        }

        public static int DeleteSubject(int id)
        {
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("DELETE FROM subjects WHERE id = @id;", connection);

            command.Parameters.AddWithValue("@id", id);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }

        #region Validations
        private static void ValidateMessage(string? message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new Exception("The message is empty!");
        }

        private static void ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("The title is empty!");
        }

        private static void ValidateId(int id)
        {
            if (id < 0)
                throw new ArgumentException("Id cannot be negative!");
        }
        #endregion
    }
}
