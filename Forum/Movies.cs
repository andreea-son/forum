using Forum.Dto;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Forum
{
    public class Movies
    {
        public static Movie? SelectMovieById(int id) 
        {
            Movie? movie = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT id, title, release_date, score, duration, genre, link, poster, description, actors FROM movies where id=@id", connection);

            command.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    movie = new Movie()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        ReleaseDate = reader.GetDateTime(2),
                        Score = reader.GetInt32(3),
                        Duration = reader.GetInt32(4),
                        Genre = reader.GetString(5),
                        Link = reader.GetString(6),
                        Poster = reader.GetString(7),
                        Description = reader.GetString(8),
                        Actors = reader.GetString(9)
                    };
                }
            }
            connection.Close();
            return movie;
        }

        public static int AddMovie(Movie movie)
        {
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("INSERT INTO movies (title, release_date, score, duration, genre, link, poster, description, actors) VALUES (@title, @release_date, @score, @duration, @genre, @link, @poster, @description, @actors);", connection);

            command.Parameters.AddWithValue("@title", movie.Title);
            command.Parameters.AddWithValue("@release_date", movie.ReleaseDate);
            command.Parameters.AddWithValue("@score", movie.Score);
            command.Parameters.AddWithValue("@duration", movie.Duration);
            command.Parameters.AddWithValue("@genre", movie.Genre);
            command.Parameters.AddWithValue("@link", movie.Link);
            command.Parameters.AddWithValue("@poster", movie.Poster);
            command.Parameters.AddWithValue("@description", movie.Description);
            command.Parameters.AddWithValue("@actors", movie.Actors);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }
    }
}
