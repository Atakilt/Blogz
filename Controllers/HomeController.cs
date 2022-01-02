using Blog.Data;
using Blog.Data.FileManager;
using Blog.Models;
using Blog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;
        public readonly IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager, ILogger<HomeController> logger)
        {
            _repo = repo;
            _fileManager = fileManager;
            _logger = logger;
        }

        [HttpGet]
        public async  Task<IActionResult> Index(string category)
        {
            var posts = string.IsNullOrEmpty(category) ? await _repo.GetAllPosts() : await _repo.GetAllPosts(category);
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {
            var post = await _repo.GetPostById(id);
            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }
        
        public IActionResult Privacy()
        {
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
