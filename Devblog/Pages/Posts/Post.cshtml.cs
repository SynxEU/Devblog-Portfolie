using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Posts
{
    public class PostModel : PageModel
    {
        private readonly IPost _postRepo;
        public PostModel(IPost repo)
        {
            _postRepo = repo;
        }

        public Post Post { get; set; }
        [BindProperty]
        public BlogPost BlogPost { get; set; }
        [BindProperty]
        public Project Project { get; set; }

        public void OnGet(Guid id)
        {
            Post = _postRepo.GetAllPosts(true).FirstOrDefault(p => p.Id == id);
        }

        public void OnPostDelete(Guid id)
        {
            _postRepo.DeletePost(id);
        }
    }
}
