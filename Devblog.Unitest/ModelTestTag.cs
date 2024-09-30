using Devblog.Domain.Model;
using Devblog.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Unitest
{
    public class ModelTestTag
    {
        [Fact]
        public void UnixTestTag()
        {
            List<Tag> tags = new List<Tag>();

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

            #region Posts (Blog)
            Post blogPost1 = new BlogPost(Guid.NewGuid(), "Alice's Blog", person1, "https://aliceblog.com", "https://aliceblog.com/weblog", false);
            #endregion

            #region Posts (Projects)
            Post project1 = new Project(Guid.NewGuid(), "Alice's Project", person1, "https://aliceproject.com", "A cool project by Alice", false);
            #endregion

            Tag tag1 = new Tag { Id = Guid.NewGuid(), Name = "Technology" };
            Tag tag2 = new Tag { Id = Guid.NewGuid(), Name = "Science" };

            tags.Add(tag1);
            tags.Add(tag2);

            Assert.NotNull(tag1);
            Assert.NotNull(tag2);

            Assert.NotEmpty(tags);

            Assert.Equal("Technology", tag1.Name);
            Assert.Equal("Science", tag2.Name);
        }
    }
}
