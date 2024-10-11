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

        public Post CreatePost(string title, Person author, string reference, bool type, string content)
            => _post.CreatePost(title, author, reference, type, content);

        public void DeletePost(Guid id)
            => _post.DeletePost(id);

        public void UpdatePost(Guid id, string newTitle = null, string newReference = null, string newContent = null)
            => _post.UpdatePost(id, newTitle, newReference, newContent);

        public void SoftDeletePost(Guid id)
            => _post.SoftDeletePost(id);

        public void UpdatePostTags(Guid postId, List<Tag> tags)
            => _post.UpdatePostTags(postId, tags);

        public void RestoreDeletedPost(Guid id)
            => _post.RestoreDeletedPost(id);

        public List<Post> GetAllPosts(bool includeDeleted = false)
            => _post.GetAllPosts(includeDeleted);

        public List<Post> GetPostsByTag(string tagName)
            => _post.GetPostsByTag(tagName);

        public Post GetPostById(Guid id)
            => _post.GetPostById(id);

        public List<Post> GetPostsByAuthorEmail(string email)
            => _post.GetPostsByAuthorEmail(email);

        public List<Post> GetPostsByAuthorId(Guid persId)
            => _post.GetPostsByAuthorId(persId);
    }
}
