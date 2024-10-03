using Devblog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Services.Interface
{
    public interface IPost
    {
        Post CreatePost(Guid id, string title, Person author, string reference, bool type, string content, List<Tag> tags);
        void DeletePost(Guid id);
        void UpdatePost(Guid id, string newTitle = null, string newReference = null, string newContent = null);
        void SoftDeletePost(Guid id);
        List<Post> GetAllPosts(bool includeDeleted = false);
        Post GetPostById(Guid id);
        List<Post> GetPostsByAuthorEmail(string email);
    }
}
