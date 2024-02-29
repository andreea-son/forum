using Forum.Dto;
using Forum.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forum.UI.Controllers
{
    public class SubjectsController : MyController
    {
        private static int CategoryId { get; set; }

        //public ActionResult AllSubjects()
        //{
        //    var temp = new List<Subject>();
        //    foreach (var subject in Subjects.GetSubjects())
        //    {
        //        temp.Add(new Subject()
        //        {
        //            Id = subject.Id,
        //            CategoryId = subject.CategoryId,
        //            Created = subject.Created,
        //            UserId = subject.UserId,
        //            Message = subject.Message,
        //            Title = subject.Title,
        //            //Replies = subject.Replies
        //        });
        //    }
        //    return View(temp);
        //}

        [HttpGet]
        public IActionResult GetSubjects(int id)
        {
            CategoryId = id;
            ViewBag.CategoryId = id;
            List<Models.Subject> temp = new();
            foreach (var subject in Subjects.GetByCategoryId(id))
            {
                temp.Add(new Models.Subject()
                {
                    Id = subject.Id,
                    CategoryId = subject.CategoryId,
                    Created = subject.Created,
                    UserId = subject.UserId,
                    Message = subject.Message,
                    Title = subject.Title,
                    TopicId = subject.TopicId
                });
            }

            List<Dto.Movie> movieList = new List<Dto.Movie>();
            List<Dto.TvSeries> tvSeriesList = new List<Dto.TvSeries>();

            string category = Categories.GetCategoryById(CategoryId)?.Name;
            
            if (category == "Movies")
            {
                foreach (Models.Subject subject in temp)
                {
                    int movieId = subject.TopicId;
                    Dto.Movie movie = Movies.SelectMovieById(movieId);
                    movieList.Add(new Dto.Movie()
                    {
                        Id = movieId,
                        Title = movie.Title,
                        ReleaseDate = movie.ReleaseDate,
                        Score = movie.Score,
                        Duration = movie.Duration,
                        Genre = movie.Genre,
                        Link = movie.Link,
                        Poster = movie.Poster,
                        Description = movie.Description,
                        Actors = movie.Actors
                    });
                }
                Dictionary<Models.Subject, Dto.Movie> myDictionary = temp.Zip(movieList, (k, v) => new { Key = k, Value = v })
                                                     .ToDictionary(x => x.Key, x => x.Value);
                ViewBag.movies_dict = myDictionary;
            }

            if (category == "TV Series")
            {
                foreach (Models.Subject subject in temp)
                {
                    int tvSeriesId = subject.TopicId;
                    Dto.TvSeries tvSeries = TvSeries.SelectTvSeriesById(tvSeriesId);
                    tvSeriesList.Add(new Dto.TvSeries()
                    {
                        Id = tvSeries.Id,
                        Title = tvSeries.Title,
                        FirstAirDate = tvSeries.FirstAirDate,
                        Score = tvSeries.Score,
                        AverageEpisodeDuration = tvSeries.AverageEpisodeDuration,
                        Genre = tvSeries.Genre,
                        Link = tvSeries.Link,
                        Poster = tvSeries.Poster,
                        Description = tvSeries.Description,
                        Actors = tvSeries.Actors,
                        NumberOfSeasons = tvSeries.NumberOfSeasons
                    });
                }
                Dictionary<Models.Subject, Dto.TvSeries> myDictionary = temp.Zip(tvSeriesList, (k, v) => new { Key = k, Value = v })
                                                     .ToDictionary(x => x.Key, x => x.Value);
                ViewBag.tv_series_dict = myDictionary;
            }

            if (category != "Movies" && category != "TV Series")
            {
                ViewBag.subjects = temp;
            }

            return View("AllSubjects", category);
        }

        [HttpPost]
        //public IActionResult AddSubject(Models.Subject model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (Subjects.AddSubject(model.Title, model.Message, CategoryId, HomeController.LoggedUser?.Id ?? 0) == 1)
        //                return RedirectToAction("GetSubjects", "Subjects", new { Id = CategoryId });
        //            else
        //                ModelState.AddModelError(string.Empty, "Error adding subject");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, ex.Message);
        //        }
        //    }
        //    return View(model);
        //}
        public async Task<IActionResult> AddSubject(Models.Subject model)
        {
            if (ModelState.IsValid)
            {
                if (CategoryId > 2)
                {
                    try
                    {
                        Subjects.AddSubject(model.Title, model.Message, CategoryId, LoggedUser.Id);
                        return RedirectToAction("GetSubjects", "Subjects", new { Id = CategoryId });
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        using (var httpClient = new HttpClient())
                        {
                            string searchUrl = "";
                            if (CategoryId == 1) // Movie
                                searchUrl = $"https://api.themoviedb.org/3/search/movie?api_key=bb8033dc88dd937b63527459ca0ed423&language=en-US&query={Uri.EscapeDataString(model.Title)}";
                            else if (CategoryId == 2) // TV series
                                searchUrl = $"https://api.themoviedb.org/3/search/tv?api_key=bb8033dc88dd937b63527459ca0ed423&language=en-US&query={Uri.EscapeDataString(model.Title)}";
                       
                            var searchResponse = await httpClient.GetAsync(searchUrl);
                            if (searchResponse.IsSuccessStatusCode)
                            {
                                var searchContent = await searchResponse.Content.ReadAsStringAsync();
                                dynamic searchData = JsonConvert.DeserializeObject(searchContent);
                                var searchResults = searchData.results;

                                if (searchResults.Count > 0)
                                {
                                    var itemId = searchResults[0].id;
                                    string itemUrl = "";
                                    string creditsUrl = "";

                                    if (CategoryId == 1) // Movie
                                    {
                                        itemUrl = $"https://api.themoviedb.org/3/movie/{itemId}?api_key=bb8033dc88dd937b63527459ca0ed423&language=en-US";
                                        creditsUrl = $"https://api.themoviedb.org/3/movie/{itemId}/credits?api_key=bb8033dc88dd937b63527459ca0ed423&language=en-US";
                                    }
                                    else if (CategoryId == 2) // TV series
                                    {
                                        itemUrl = $"https://api.themoviedb.org/3/tv/{itemId}?api_key=bb8033dc88dd937b63527459ca0ed423&language=en-US";
                                        creditsUrl = $"https://api.themoviedb.org/3/tv/{itemId}/credits?api_key=bb8033dc88dd937b63527459ca0ed423&language=en-US";
                                    }

                                    var itemResponse = await httpClient.GetAsync(itemUrl);
                                    var creditsResponse = await httpClient.GetAsync(creditsUrl);

                                    if (itemResponse.IsSuccessStatusCode && creditsResponse.IsSuccessStatusCode)
                                    {
                                        var itemContent = await itemResponse.Content.ReadAsStringAsync();
                                        var creditsContent = await creditsResponse.Content.ReadAsStringAsync();
                                        dynamic itemData = JsonConvert.DeserializeObject(itemContent);
                                        dynamic creditsData = JsonConvert.DeserializeObject(creditsContent);

                                        var actors = string.Join(", ", ((JArray)creditsData.cast).Take(5).Select(c => (string)c["name"]).ToList());

                                        if (CategoryId == 1) // Movie
                                        {
                                            Dto.Movie movie = new Dto.Movie
                                            {
                                                Title = itemData.title,
                                                ReleaseDate = itemData.release_date,
                                                Score = (float)itemData.vote_average,
                                                Duration = itemData.runtime,
                                                Genre = string.Join(", ", ((JArray)itemData.genres).Select(g => (string)g["name"]).ToList()),
                                                Link = $"https://www.themoviedb.org/movie/{itemId}",
                                                Poster = $"https://image.tmdb.org/t/p/original{itemData.poster_path}",
                                                Description = itemData.overview,
                                                Actors = actors
                                            };
                                            Movies.AddMovie(movie);
                                            Subjects.AddSubject(model.Title, model.Message, CategoryId, LoggedUser.Id);
                                        }
                                        else if (CategoryId == 2) // TV series
                                        {
                                            Dto.TvSeries tvSeries = new Dto.TvSeries
                                            {
                                                Title = itemData.name,
                                                FirstAirDate = itemData.first_air_date,
                                                Score = (float)itemData.vote_average,
                                                AverageEpisodeDuration = itemData.episode_run_time[0],
                                                Genre = string.Join(", ", ((JArray)itemData.genres).Select(g => (string)g["name"]).ToList()),
                                                Link = $"https://www.themoviedb.org/tv/{itemId}",
                                                Poster = $"https://image.tmdb.org/t/p/original{itemData.poster_path}",
                                                Description = itemData.overview,
                                                Actors = actors,
                                                NumberOfSeasons = itemData.number_of_seasons
                                            };
                                            TvSeries.AddTvSeries(tvSeries);
                                            Subjects.AddSubject(model.Title, model.Message, CategoryId, LoggedUser.Id);
                                        }
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Item not found");
                                }
                            }
                        }
                        return RedirectToAction("GetSubjects", "Subjects", new { Id = CategoryId });
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddSubject(int id)
        {
            CategoryId = id;
            return View();
        }

        public ActionResult DeleteSubject(int id)
        {
            Subjects.DeleteSubject(id);
            return RedirectToAction("GetSubjects", "Subjects", new { Id = CategoryId });
        }
    }
}
