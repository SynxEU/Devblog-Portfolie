using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Domain.Model
{
    public class TagList
    {
        public Guid PostsId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
 