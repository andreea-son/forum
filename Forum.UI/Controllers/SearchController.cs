using Forum.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Forum.UI.Controllers
{
    public class SearchController : MyController
    {
        public IActionResult SearchForum(string searchTerm)
        {   try { 
                var temp = new List<Search>();
                foreach (var result in Searching.SearchForum(searchTerm))
                {
                    temp.Add(new Search()
                    {
                        Id = result.Id,
                        Result = result.Result,
                        IsCategory = result.IsCategory,
                        IsReply = result.IsReply,
                        IsSubject = result.IsSubject
                    });
                }
                return View("Searching", temp);
            } catch (Exception){ }
            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public IActionResult GoToResult(int id)
        {
            //try
            //{

            //    if (isCategory)
            //        return RedirectToAction("GetSubjects", "Subjects", new { id });
            //    else if (isSubject)
            //        return RedirectToAction("GetReplies", "Replies", new { id });
            //    else if (isReply)
            //        throw new Exception();
            //    return RedirectToAction("GetSubjects", "GetSubjects", new { id });
            //}
            //catch (Exception) { }
            return RedirectToAction("GetReplies", "Replies", new { id });
        }
    }
}
