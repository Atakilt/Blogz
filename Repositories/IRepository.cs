using Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repositories
{
    public interface IRepository
    {
        Task<Post> GetPostById(int id);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetAllPosts(string category);
        Task AddPost(Post post);
        Task RemovePost(int id);
        void UpdatePost(Post post);

        Task<bool> SaveChangesAsync();
    }
}
