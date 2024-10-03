using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Devblog.Domain.Model;
using Devblog.Services.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Devblog.Pages.Author
{
    public class ProfileModel : PageModel
    {
        private readonly IPerson _personRepo;
        private readonly IPost _postRepo;

        public ProfileModel(IPerson personRepo, IPost postRepo)
        {
            _personRepo = personRepo;
            _postRepo = postRepo;
        }

        [BindProperty]
        public Person Person { get; set; }

        public List<Post> UserPosts { get; set; }

        public IActionResult OnGet(Guid personId)
        {
            string idString = HttpContext.Session.GetString("ID");

            if (!Guid.TryParse(idString, out Guid userId))
            {
                return Redirect("/Login");
            }

            Person = _personRepo.GetPersonById(userId);

            if (Person == null)
            {
                return Redirect("/Login"); 
            }

            UserPosts = _postRepo.GetPostsByAuthorEmail(Person.Email);
            return Page();
        }

        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _personRepo.UpdatePerson(Person.Id, Person.FirstName, Person.LastName, Person.Age, Person.Password, Person.City, Person.PhoneNumber, Person.LinkedIn, Person.Github);
            return RedirectToPage();
        }
    }
}
