using Forum.Dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class Categories
    {
        public static int AddCategory(string? name, string? description)
        {
            ValidateName(name);
            if (GetCategory(name) != null)
                throw new Exception("The category exists!");
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("INSERT INTO categories (name, description) VALUES (@name, @description);", connection);

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }

        public static Category? GetCategory(string? name)
        {
            ValidateName(name);
            Category? category = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM categories WHERE name = @name;", connection);

            command.Parameters.AddWithValue("@name", name);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    category = new Category()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = !reader.IsDBNull(2) ? reader.GetString(2) : null
                    };
                }
            }
            connection.Close();
            return category;
        }

        public static Category? GetCategoryById(int id)
        {
            Category? category = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM categories WHERE id = @id;", connection);

            command.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    category = new Category()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = !reader.IsDBNull(2) ? reader.GetString(2) : null
                    };
                }
            }
            connection.Close();
            return category;
        }

        public static List<Category> GetCategories()
        {
            List<Category> categories = new();
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT * FROM categories;", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    categories.Add(new Category()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = !reader.IsDBNull(2) ? reader.GetString(2) : null
                    });
                }
            }
            connection.Close();
            return categories;
        }

        public static int DeleteCategory(int id)
        {
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("START TRANSACTION; " +
                "DELETE FROM replies WHERE subject_id IN (SELECT id FROM subjects WHERE category_id = @id); " +
                "DELETE FROM subjects WHERE category_id = @id; " +
                "DELETE FROM categories WHERE id = @id; " +
                "COMMIT;", connection);

            command.Parameters.AddWithValue("@id", id);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }

        private static void ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("The name is empty!");
        }
    }
}
