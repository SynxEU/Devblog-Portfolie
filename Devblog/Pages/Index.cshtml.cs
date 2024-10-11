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
        public Person Person { get; set; }

        private readonly IPost _postRepo;
        private readonly IPerson _personRepo;

        public IndexModel(IPost repo, IPerson person)
        {
            _postRepo = repo;
            _personRepo = person;
        }

        public void OnGet()
        {
            var allPosts = _postRepo.GetAllPosts(true);

            BlogPosts = allPosts.OfType<BlogPost>().ToList();
            Projects = allPosts.OfType<Project>().ToList();

            foreach (var post in allPosts)
            {
                Person = _personRepo.GetPersonById(post.Author.Id);
            }
        }
    }
}
