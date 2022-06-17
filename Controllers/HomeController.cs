using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Controllers
{
    public class HomeController : Controller
    {
        public readonly SocialMediaAppContext _context;

        public HomeController(SocialMediaAppContext context)
        {
            _context = context;
        }
        public IActionResult Feed()
        {
            return View();
        }
        [HttpGet] public ActionResult GetFeed()
        {
            var q = _context.PostsModel!.ToList();
            return Json(new { Data = q });
        }
        [HttpGet] public ActionResult GetPost(int id)
        {
            var q = from p in _context.PostsModel! where p.ID == id select p;
            return Json(new { Data = q });
        }
        [HttpPost] public ActionResult EditPost(int id, string content, PostsModel postModel)
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

            return Json(new { Data = postModel } );
        }
        [HttpPost] public ActionResult DeletePost(int id)
        {
            var postsModel = _context.PostsModel!.Find(id);
            if(postsModel != null)
            {
                _context.PostsModel.Remove(postsModel);
                _context.SaveChanges();
            }
            return Json(new { Data = postsModel } );
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}
