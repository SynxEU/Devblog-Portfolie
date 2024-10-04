using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Posts
{
    public class PostModel : PageModel
    {
        private readonly IPost _postRepo;
        private readonly IPerson _personRepo;

        public PostModel(IPost repo, IPerson repoPerson)
        {
            _postRepo = repo;
            _personRepo = repoPerson;
        }

        public Post Post { get; set; }
        [BindProperty]
        public BlogPost BlogPost { get; set; }
        [BindProperty]
        public Project Project { get; set; }
        public string Email { get; set; }

        public IActionResult OnGet(Guid id)
        {
            string idString = HttpContext.Session.GetString("ID");
            
            Console.WriteLine(idString);

            if (!string.IsNullOrEmpty(idString) && Guid.TryParse(idString, out Guid userId))
            {
                Email = _personRepo.GetPersonById(userId)?.Email;
            }

            Post = _postRepo.GetAllPosts(true).FirstOrDefault(p => p.Id == id);

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPostDelete(Guid postId)
        {
            Console.WriteLine(postId);

            _postRepo.DeletePost(postId);

            return RedirectToPage("/Index");
        }

    }
}
