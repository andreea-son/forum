using Forum.Dto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum
{
    public class TvSeries
    {
        public static Forum.Dto.TvSeries? SelectTvSeriesById(int id)
        {
            Forum.Dto.TvSeries? tvSeries = null;
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("SELECT id, title, First_Air_Date, score, Average_Episode_Duration, genre, link, poster, description, actors, Number_of_Seasons FROM tv_series where id=@id", connection);

            command.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tvSeries = new Forum.Dto.TvSeries()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        FirstAirDate = reader.GetDateTime(2),
                        Score = reader.GetInt32(3),
                        AverageEpisodeDuration = reader.GetInt32(4),
                        Genre = reader.GetString(5),
                        Link = reader.GetString(6),
                        Poster = reader.GetString(7),
                        Description = reader.GetString(8),
                        Actors = reader.GetString(9),
                        NumberOfSeasons = reader.GetInt32(10)
                    };
                }
            }
            connection.Close();
            return tvSeries;
        }

        public static int AddTvSeries(Dto.TvSeries tvSeries)
        {
            MySqlConnection connection = new(Settings.connString);
            connection.Open();
            MySqlCommand command = new("INSERT INTO tv_series (title, First_Air_Date, score, Average_Episode_Duration, genre, link, poster, description, actors, Number_of_Seasons) VALUES (@title, @First_Air_Date, @score, @Average_Episode_Duration, @genre, @link, @poster, @description, @actors, @Number_of_Seasons);", connection);

            command.Parameters.AddWithValue("@title", tvSeries.Title);
            command.Parameters.AddWithValue("@release_date", tvSeries.FirstAirDate);
            command.Parameters.AddWithValue("@score", tvSeries.Score);
            command.Parameters.AddWithValue("@duration", tvSeries.AverageEpisodeDuration);
            command.Parameters.AddWithValue("@genre", tvSeries.Genre);
            command.Parameters.AddWithValue("@link", tvSeries.Link);
            command.Parameters.AddWithValue("@poster", tvSeries.Poster);
            command.Parameters.AddWithValue("@description", tvSeries.Description);
            command.Parameters.AddWithValue("@actors", tvSeries.Actors);
            command.Parameters.AddWithValue("@actors", tvSeries.NumberOfSeasons);

            int newRows = command.ExecuteNonQuery();

            connection.Close();
            return newRows;
        }
    }
}
