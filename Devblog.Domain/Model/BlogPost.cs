using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Domain.Model
{
    public class BlogPost : Post
    {
        public string Weblog { get; set; }
    
        public BlogPost(Guid id, string title, Person author, string reference, string weblog, bool isDeleted) : base(id, title, author, reference, isDeleted)
        {
            this.Weblog = weblog;
        }
    }
}
