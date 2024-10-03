using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

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
        public BlogPost BlogPost { get; set; }

        [BindProperty]
        public Project Project { get; set; }

        public IActionResult OnGet(Guid id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("ID")))
            {
                return Redirect("/Login");
            }

            var post = _postRepo.GetAllPosts(true).FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            if (post is BlogPost blogPost)
            {
                BlogPost = blogPost;
            }
            else if (post is Project project)
            {
                Project = project;
            }

            return Page();
        }

        public IActionResult OnPost(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (BlogPost != null)
            {
                _postRepo.UpdatePost(id, BlogPost.Title, BlogPost.Reference, BlogPost.Weblog);
            }
            else if (Project != null)
            {
                _postRepo.UpdatePost(id, Project.Title, Project.Reference, Project.Description);
            }

            return RedirectToPage("/Posts/Post", new { id });
        }
    }
}
