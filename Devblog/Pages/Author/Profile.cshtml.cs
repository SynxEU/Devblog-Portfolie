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

        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

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

            UserPosts = _postRepo.GetPostsByAuthorId(userId);
            return Page();
        }

        public IActionResult OnPostChangePassword()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword))
            {
                ModelState.AddModelError("", "Both passwords are required.");
                return Page();
            }

            Person = _personRepo.GetPersonById(Person.Id);

            if (Person.Password != CurrentPassword)
            {
                ModelState.AddModelError("", "Current password is incorrect.");
                return Page();
            }

            _personRepo.UpdatePerson(Person.Id, Person.FirstName, Person.LastName, Person.Age, Person.City, Person.PhoneNumber, Person.LinkedIn, Person.Github);

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToPage("/index");
        }
        public IActionResult OnPostDeletePost(Guid postId)
        {
            Console.WriteLine(postId);
            _postRepo.DeletePost(postId);

            return RedirectToPage();
        }

        public IActionResult OnPostSoftDeletePost(Guid postId)
        {
            Console.WriteLine(postId);
            _postRepo.SoftDeletePost(postId);

            return RedirectToPage();
        }
        public IActionResult OnPostRestoreDeletePost(Guid postId)
        {
            Console.WriteLine(postId);
            _postRepo.RestoreDeletedPost(postId);

            return RedirectToPage();
        }

    }
}
