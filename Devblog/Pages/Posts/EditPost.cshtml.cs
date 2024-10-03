using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Posts
{
    public class EditPostModel : PageModel
    {
        private readonly IPost _postRepo;
        public EditPostModel(IPost repo)
        {
            _postRepo = repo;
        }

        [BindProperty]
        public Post Post { get; set; }
        [BindProperty]
        public BlogPost BlogPost { get; set; }
        [BindProperty]
        public Project Project { get; set; }

        public IActionResult OnGet(Guid id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("ID")))
            {
                return Redirect("/Login");
            }

            Post = _postRepo.GetAllPosts(true).FirstOrDefault(p => p.Id == id);

            if (Post is BlogPost blogPost)
            {
                BlogPost = blogPost;
            }
            else if (Post is Project project)
            {
                Project = project;
            }
            return Page();
        }

        public void OnPost(Guid id)
        {
            if (Post is BlogPost)
            {
                _postRepo.UpdatePost(id, Post.Title, Post.Reference, BlogPost.Weblog);
            }
            else if (Post is Project)
            {
                _postRepo.UpdatePost(id, Post.Title, Post.Reference, Project.Description);
            }
        }
    }
}
