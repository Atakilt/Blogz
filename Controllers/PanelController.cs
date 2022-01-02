using Blog.Data.FileManager;
using Blog.Models;
using Blog.Repositories;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private readonly IRepository _repo;
        public readonly IFileManager _fileManager;

        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            var posts = await _repo.GetAllPosts();
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                 return View(new PostViewModel());
            }                
            else
            {
                var post = await _repo.GetPostById((int)id);
                var vm = new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    Description = post.Description,
                    Tags = post.Tags,
                    Category = post.Category,
                    CurrentImage = post.Image
                };
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                Description = vm.Description,
                Tags = vm.Tags,
                Category = vm.Category,               
            };

            if (vm.Image == null)
                post.Image = vm.CurrentImage;
            else
                post.Image = await _fileManager.SaveImage(vm.Image);

            if (post.Id > 0)
                _repo.UpdatePost(post);

            else
                await _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }

            return View(post);

        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            await _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
