using Forum.Dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class Replies
    {
        public static int AddReply(string? message, int subjectId, int userId)
        {
            ValidateMessage(message);
            ValidateId(subjectId);
            ValidateId(userId);
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("INSERT INTO replies (message, subject_id, user_id) VALUES (@message, @subjectId, @userId);", connection);

            command.Parameters.AddWithValue("@message", message);
            command.Parameters.AddWithValue("@subjectId", subjectId);
            command.Parameters.AddWithValue("@userId", userId);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }
        public static void EditReply(Reply reply)
        {
            ValidateId(reply.Id);
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("UPDATE replies set message = @message WHERE id = @id", connection);
            command.Parameters.AddWithValue("@message", reply.Message);
            command.Parameters.AddWithValue("@id", reply.Id);
            command.ExecuteNonQuery();

            connection.Close();
        }
        public static List<Reply> GetBySubjectId(int subjectId)
        {
            ValidateId(subjectId);
            List<Reply> subjects = new();
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT r.id, r.subject_id, r.user_id, r.message, r.created, u.username " +
                "FROM replies AS r " +
                "LEFT JOIN users AS u ON u.id = r.user_id " +
                "WHERE r.subject_id = @subjectId;", connection);

            command.Parameters.AddWithValue("@subjectId", subjectId);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    subjects.Add(new Reply()
                    {
                        Id = reader.GetInt32(0),
                        SubjectId = reader.GetInt32(1),
                        UserId = reader.GetInt32(2),
                        Message = reader.GetString(3),
                        Created = reader.GetDateTime(4),
                        Username = !reader.IsDBNull(5) ? reader.GetString(5) : "Anonymous"
                    });
                }
            }
            connection.Close();
            return subjects;
        }

        public static Reply? GetReply(int id)
        {
            ValidateId(id);
            Reply? subject = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM replies WHERE id = @id;", connection);

            command.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    subject = new Reply()
                    {
                        Id = reader.GetInt32(0),
                        Message = reader.GetString(1),
                        SubjectId = reader.GetInt32(2),
                        UserId = reader.GetInt32(3),
                        Created = reader.GetDateTime(4)
                    };
                }
            }
            connection.Close();
            return subject;
        }

        public static List<Reply> GetReplies()
        {
            List<Reply> subjects = new();
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM replies;", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    subjects.Add(new Reply()
                    {
                        Id = reader.GetInt32(0),
                        SubjectId = reader.GetInt32(1),
                        UserId = reader.GetInt32(2),
                        Message = reader.GetString(3),
                        Created = reader.GetDateTime(4)
                    });
                }
            }
            connection.Close();
            return subjects;
        }

        public static int DeleteReply(int id)
        {
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("DELETE FROM replies WHERE id = @id;", connection);

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

        private static void ValidateId(int id)
        {
            if (id < 0)
                throw new ArgumentException("Id cannot be negative!");
        }
        #endregion
    }
}
