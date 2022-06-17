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

        // GET: PostsModels
        public async Task<IActionResult> Index()
        {
              return _context.PostsModel != null ? 
                          View(await _context.PostsModel.ToListAsync()) :
                          Problem("Entity set 'SocialMediaAppContext.PostsModel'  is null.");
        }
        // GET: PostsModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostsModel == null)
            {
                return NotFound();
            }

            var postsModel = await _context.PostsModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (postsModel == null)
            {
                return NotFound();
            }

            return View(postsModel);
        }

        // GET: PostsModels/Create
        public IActionResult Create()
        {
            Console.WriteLine(AccountsModelsController.onAdminMode);
            if(AccountsModelsController.onAdminMode == false)
            {
                return RedirectToAction("Feed", "Home");
            }
            else
            {
                return View();
            }
        }

        // POST: PostsModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,post,date,postedBy")] PostsModel postsModel, string post)
        {
            Console.WriteLine(AccountsModelsController.onAdminMode);
            if (ModelState.IsValid)
            {
                if(AccountsModelsController.onAdminMode == false)
                {
                    _context.Add(PostToFeed(post));
                }
                else
                {
                    _context.Add(postsModel);
                }
                await _context.SaveChangesAsync();
                if (AccountsModelsController.onAdminMode == false)
                {
                    return RedirectToAction("Feed", "Home");
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(postsModel);
        }

        // GET: PostsModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostsModel == null)
            {
                return NotFound();
            }

            var postsModel = await _context.PostsModel.FindAsync(id);
            if (postsModel == null)
            {
                return NotFound();
            }
            return View(postsModel);
        }

        // POST: PostsModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,content,date,postedBy")] PostsModel postsModel)
        {
            if (id != postsModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsModelExists(postsModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(postsModel);
        }

        // GET: PostsModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostsModel == null)
            {
                return NotFound();
            }

            var postsModel = await _context.PostsModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (postsModel == null)
            {
                return NotFound();
            }

            return View(postsModel);
        }

        // POST: PostsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostsModel == null)
            {
                return Problem("Entity set 'SocialMediaAppContext.PostsModel'  is null.");
            }
            var postsModel = await _context.PostsModel.FindAsync(id);
            if (postsModel != null)
            {
                _context.PostsModel.Remove(postsModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostsModelExists(int id)
        {
          return (_context.PostsModel?.Any(e => e.ID == id)).GetValueOrDefault();
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
    }
}
