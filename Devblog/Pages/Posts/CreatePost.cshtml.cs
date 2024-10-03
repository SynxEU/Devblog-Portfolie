using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Devblog.Pages.Posts
{
    public class CreatePostModel : PageModel
    {
        private readonly IPost _postRepo;
        private readonly IPerson _personRepo;
        public Person Author { get; set; } = new Person();

        public CreatePostModel(IPost repo, IPerson repoPerson)
        {
            _postRepo = repo;
            _personRepo = repoPerson;
        }

        [BindProperty]
        [MaxLength(25)]
        public string Title { get; set; }
        [BindProperty]
        public string Reference { get; set; }
        [BindProperty]
        public bool IsBlog { get; set; }
        [BindProperty]
        [MaxLength(5000)]
        public string Content { get; set; }
        [BindProperty]
        public string Tags { get; set; }

        public IActionResult OnGet()
        {
            string id = HttpContext.Session.GetString("ID");

            if (!Guid.TryParse(id, out Guid userId))
            {
                return Redirect("/Login");
            }

            Author = _personRepo.GetPersonById(userId);

            Console.WriteLine(id);
            Console.WriteLine(Author.Fullname);
            Console.WriteLine(Author.Email);

            return Page();
        }

        public IActionResult OnPost()
        {
            List<Tag> tagList = Tags?.Split(',').Select(t => new Tag { Name = t.Trim() }).ToList() ?? new List<Tag>();
            _postRepo.CreatePost(Guid.Empty, Title, Author, Reference, IsBlog, Content, tagList);
            return RedirectToPage("/Posts");
        }
    }
}
