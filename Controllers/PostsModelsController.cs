using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Controllers
{
    public class PostsModelsController : Controller
    {
        public readonly SocialMediaAppContext _context;
        public static PostsModel? postModel;

        public PostsModelsController(SocialMediaAppContext context)
        {
            _context = context;
        }
        public IActionResult Feed()
        {
            return View();
        }
        public PostsModel PostToFeed(string content)
        {
            postModel = new PostsModel {
                content = content,
                date = DateTime.Now.ToString(),
                postedBy = AccountsModelsController._username
            };
            return postModel;
        }
        [HttpPost]
        public ActionResult CreatePost(string content)
        {
            var p = new PostsModel
            {
                content = content,
                date = DateTime.Now.ToString(),
                postedBy = AccountsModelsController._username
            };
            _context.Add(p);
            _context.SaveChanges();
            Console.WriteLine(p);
            return new JsonResult(p);
        }
        [HttpGet]
        public ActionResult GetFeed()
        {
            var q = _context.PostsModel!.ToList();
            return Json(new { Data = q });
        }
        [HttpGet]
        public ActionResult GetPost(int id)
        {
            var q = from p in _context.PostsModel! where p.ID == id select p;
            return Json(new { Data = q });
        }
        [HttpPost]
        public ActionResult EditPost(int id, string content, PostsModel postModel)
        {
            if (postModel is null)
            {
                throw new ArgumentNullException(nameof(postModel));
            }

            postModel = new PostsModel
            {
                ID = id,
                content = content,
                postedBy = AccountsModelsController._username,
                date = "Edited " + DateTime.Now.ToString()
            };

            if (ModelState.IsValid)
            {
                _context.Update(postModel);
                _context.SaveChanges();
            }

            return Json(new { Data = postModel });
        }
        [HttpPost]
        public ActionResult DeletePost(int id)
        {
            var postsModel = _context.PostsModel!.Find(id);
            if (postsModel != null)
            {
                _context.PostsModel.Remove(postsModel);
                _context.SaveChanges();
            }
            return Json(new { Data = postsModel });
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}
