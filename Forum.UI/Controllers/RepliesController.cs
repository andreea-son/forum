using Forum.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Forum.UI.Controllers
{
    public class RepliesController : MyController
    {
        private static int subjectId;

        [HttpGet]
        public IActionResult GetReplies(int id)
        {
            subjectId = id;
            List<Reply> temp = new();

            temp.Add(new Reply()
            {
                SubjectId = subjectId
            });
            foreach (var reply in Replies.GetBySubjectId(id))
            {
                temp.Add(new Reply()
                {
                    Id = reply.Id,
                    Created = reply.Created,
                    UserId = reply.UserId,
                    Message = reply.Message,
                    SubjectId = reply.SubjectId,
                    Username = reply.Username
                });
            }
            Dto.Subject? subject = Subjects.GetSubject(subjectId);
            int categoryId = subject.CategoryId;

            string category = Categories.GetCategoryById(categoryId)?.Name;
            ViewBag.category = category;

            if (category == "Movies")
            {
                int movieId = subject.TopicId;
                ViewBag.movie = Movies.SelectMovieById(movieId);
            }

            if (category == "TV Series")
            {
                int tvSeriesId = subject.TopicId;
                ViewBag.tv_series = TvSeries.SelectTvSeriesById(tvSeriesId);
            }

            return View("AllReplies", temp);
        }

        [HttpGet]
        public IActionResult AddReply()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddReply(Reply model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Replies.AddReply(model.Message, subjectId, HomeController.LoggedUser?.Id ?? 0) == 1)
                        return RedirectToAction("GetReplies", "Replies", new { Id = subjectId });
                    else
                        ModelState.AddModelError(string.Empty, "Error adding reply");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult DeleteReply(int id)
        {
            Replies.DeleteReply(id);
            return RedirectToAction("GetReplies", "Replies", new { Id = subjectId });
        }

        [HttpGet]
        public ActionResult EditReply(int id)
        {
            return View("EditReply", Replies.GetReply(id));
        }

        [HttpPost]
        public ActionResult EditReply(Dto.Reply model)
        {
            Replies.EditReply(model);
            return RedirectToAction("GetReplies", "Replies", new { Id = subjectId });
        }
    }
}
