using Devblog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Unitest
{
    public class ModelTestProjects
    {
        [Fact]
        public void UnixTestProject()
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

            Post projectBost = new Project(Guid.NewGuid(), "Alice's Project", person1, "https://aliceblog.com", "https://aliceblog.com/weblog", false);

            posts.Add(projectBost);

            Assert.NotNull(projectBost);

            Assert.NotEmpty(posts);

            Assert.Equal("Alice's Project", projectBost.Title);
        }
    }
}
