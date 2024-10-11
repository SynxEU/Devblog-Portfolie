using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Devblog.Pages.Posts
{
    public class CreatePostModel : PageModel
    {
        private readonly IPost _postRepo;
        private readonly IPerson _personRepo;
        private readonly ITag _tagRepo;

        public Person Author { get; private set; }
        public List<Tag> AvailableTags { get; private set; } = new List<Tag>();

        public CreatePostModel(IPost repo, IPerson repoPerson, ITag tagRepo)
        {
            _postRepo = repo;
            _personRepo = repoPerson;
            _tagRepo = tagRepo;
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
        public List<Guid> SelectedTagIds { get; set; } = new List<Guid>();

        public IActionResult OnGet()
        {
            string id = HttpContext.Session.GetString("ID");

            if (!Guid.TryParse(id, out Guid userId))
            {
                return Redirect("/Login");
            }

            Author = _personRepo.GetPersonById(userId);
            //AvailableTags = _tagRepo.GetAllTags();
            //Console.WriteLine(AvailableTags.Count());
            //foreach (Tag tag in AvailableTags)
            //{
            //    Console.WriteLine($"Id: {tag.Id}\nNavn: {tag.Name}");
            //}

            return Page();
        }

        public IActionResult OnPost()
        {
            string id = HttpContext.Session.GetString("ID");
            if (!Guid.TryParse(id, out Guid userId))
            {
                return Redirect("/Login");
            }

            Author = _personRepo.GetPersonById(userId);

            //List<Tag> selectedTags = _tagRepo.GetTagsByIds(SelectedTagIds);

            // _postRepo.CreatePost(Title, Author, Reference, IsBlog, Content, selectedTags);

            _postRepo.CreatePost(Title, Author, Reference, IsBlog, Content);

            return RedirectToPage("/Posts");
        }
    }
}
