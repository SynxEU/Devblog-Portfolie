using Devblog.Domain.Model;
using Devblog.Services.Interface;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Unitest
{
    public class ModelTestsBlog
    {
        [Fact]
        public void UnixTestBlog()
        {
            List<Post> posts = new List<Post>();

            #region Persons
            Person person1 = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                Age = 30,
                Email = "alice.johnson@example.com",
                Password = "password123",
                City = "New York",
                PhoneNumber = "123-456-7890",
                LinkedIn = "alicejohnson",
                Github = "aliceghub",
                Posts = new List<Post>()
            };
            #endregion

            Post blogPost = new BlogPost(Guid.NewGuid(), "Alice's Blog", person1, "https://aliceblog.com", "https://aliceblog.com/weblog", false);
            
            posts.Add(blogPost);

            Assert.NotNull(blogPost);

            Assert.NotEmpty(posts);

            Assert.Equal("Alice's Blog", blogPost.Title);
        }
    }
}
