using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devblog.Services.Interface;
using Devblog.Domain.Model;
using Devblog.Domain.Repo;

namespace Devblog.Services.Method
{
    public class PostMethod : IPost
    {
        private readonly PostRepo _post;

        public PostMethod()
        {
            _post = new PostRepo();
        }

        public Post CreatePost(Guid id, string title, Person author, string reference, bool type, string content, List<Tag> tags)
            => _post.CreatePost(id, title, author, reference, type, content, tags);
        public void DeletePost(Guid id)
            => _post.DeletePost(id);
        public void UpdatePost(Guid id, string newTitle = null, string newReference = null, string newContent = null)
            => _post.UpdatePost(id, newTitle, newReference, newContent);
        public void SoftDeletePost(Guid id)
            => _post.SoftDeletePost(id);
        public List<Post> GetAllPosts(bool includeDeleted = false)
            => _post.GetAllPosts(includeDeleted);
        public Post GetPostById(Guid id)
            => _post.GetPostById(id);
        public List<Post> GetPostsByAuthorEmail(string email)
            => _post.GetPostsByAuthorEmail(email);
    }
}
