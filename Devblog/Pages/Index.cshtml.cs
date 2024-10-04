using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages
{
    public class IndexModel : PageModel
    {
        public List<BlogPost> BlogPosts { get; set; }
        public List<Project> Projects { get; set; }

        private readonly IPost _postRepo;

        public IndexModel(IPost repo)
        {
            _postRepo = repo;
        }

        public void OnGet()
        {
            var allPosts = _postRepo.GetAllPosts(true);

            BlogPosts = allPosts.OfType<BlogPost>().ToList();
            Projects = allPosts.OfType<Project>().ToList();
        }
    }
}
