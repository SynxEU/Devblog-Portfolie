using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Posts
{
    public class CreatePostModel : PageModel
    {
        private readonly IPost _postRepo;

        public CreatePostModel(IPost repo)
        {
            _postRepo = repo;
        }

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public Person Author { get; set; } = new Person();
        [BindProperty]
        public string Reference { get; set; }
        [BindProperty]
        public bool IsBlog { get; set; }
        [BindProperty]
        public string Content { get; set; }
        [BindProperty]
        public string Tags { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            List<Tag> tagList = Tags?.Split(',').Select(t => new Tag { Name = t.Trim() }).ToList() ?? new List<Tag>();
            _postRepo.CreatePost(Guid.Empty, Title, Author, Reference, IsBlog, Content, tagList);
        }
    }
}
