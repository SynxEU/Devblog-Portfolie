using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Domain.Model
{
    public class Project : Post
    {
        public string Description { get; set; }
        public Project(Guid id, string title, Person author, string reference, string description, bool isDeleted) : base(id, title, author, reference, isDeleted)
        {
            this.Description = description;
        }
    }
}
