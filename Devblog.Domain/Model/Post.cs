using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Domain.Model
{
    public abstract class Post
    {
        public Guid Id { get; protected set; }
        public string Title { get; protected set; }
        public Person Author { get; protected set; }
        public string Reference { get; protected set; }
        public bool IsDeleted { get; protected set; }

        protected Post(Guid id, string title, Person author, string reference, bool isDeleted)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.Reference = reference;
            this.IsDeleted = isDeleted;
        }
    }
}
