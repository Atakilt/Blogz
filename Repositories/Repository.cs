using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Repositories
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task AddPost(Post post)
        {
           await  _ctx.Posts.AddAsync(post);
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _ctx.Posts.ToListAsync();
        }

        public async Task<List<Post>> GetAllPosts(string category)
        {
            return await _ctx.Posts
                .Where(p=>p.Category.ToLower().Equals(category.ToLower()))
                .ToListAsync();
        }

        public async Task<Post> GetPostById(int id)
        {
           return await _ctx.Posts.FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task RemovePost(int id)
        {
            _ctx.Posts.Remove(await GetPostById(id));
        }
        
        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if(await _ctx.SaveChangesAsync()>0)
            {
                return true;
            }
            return false;
        }

    }
}
