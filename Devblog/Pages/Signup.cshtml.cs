using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Devblog.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace Devblog.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IPerson _repo;

        [BindProperty]
        [Required]
        public string FirstName { get; set; }

        [BindProperty]
        [Required]
        public string LastName { get; set; }

        [BindProperty]
        [Required]
        [Range(1, 120, ErrorMessage = "Please enter a valid age.")]
        public int Age { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        [BindProperty]
        public string City { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string LinkedIn { get; set; }

        [BindProperty]
        public string Github { get; set; }

        public SignupModel(IPerson repo)
        {
            _repo = repo;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newPerson = _repo.CreatePerson(Guid.NewGuid(), FirstName, LastName, Age, Email, Password, City, PhoneNumber, LinkedIn, Github);

            HttpContext.Session.SetString("ID", newPerson.Id.ToString());

            return RedirectToPage("/Author/Profile");
        }
        public void OnGet()
        {
        }
    }
}
