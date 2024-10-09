using Devblog.Domain.Model;
using Devblog.Domain.Model.View;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public BlogPostView BlogPost { get; set; }

        [BindProperty]
        public ProjectView Project { get; set; }

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
                BlogPost = new BlogPostView
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Reference = blogPost.Reference,
                    Weblog = blogPost.Weblog
                };
                ModelState.Remove(nameof(Project.Title));
                ModelState.Remove(nameof(Project.Reference));
                ModelState.Remove(nameof(Project.Description));
            }
            else if (post is Project project)
            {
                Project = new ProjectView
                {
                    Id = project.Id,
                    Title = project.Title,
                    Reference = project.Reference,
                    Description = project.Description
                };
                ModelState.Remove(nameof(BlogPost.Title));
                ModelState.Remove(nameof(BlogPost.Reference));
                ModelState.Remove(nameof(BlogPost.Weblog));
            }

            return Page();
            
        }

        public IActionResult OnPost(Guid id)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine(ModelState.ErrorCount);
                return Page();
            }

            var post = _postRepo.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            if (post is BlogPost && BlogPost != null)
            {
                
                _postRepo.UpdatePost(id, BlogPost.Title, BlogPost.Reference, BlogPost.Weblog);
            }
            else if (post is Project && Project != null)
            {
                _postRepo.UpdatePost(id, Project.Title, Project.Reference, Project.Description);
            }

            return RedirectToPage("/Posts/Post", new { id });
        }

    }
}
