using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Domain.Model.View
{
    public class ProjectView
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Reference is required")]
        [StringLength(200, ErrorMessage = "Reference cannot be longer than 200 characters")]
        public string Reference { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(5000, ErrorMessage = "Description cannot be longer than 5000 characters")]
        public string Description { get; set; }
    }
}
