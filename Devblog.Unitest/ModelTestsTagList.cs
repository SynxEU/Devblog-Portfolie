using Devblog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Unitest
{
    public class ModelTestsTagList
    {
        [Fact]
        public void TagsListUnixTests()
        {
            List<TagList> taglists = new List<TagList>();

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

            #region Tag
            Tag tag1 = new Tag { Id = Guid.NewGuid(), Name = "Technology" };
            Tag tag2 = new Tag { Id = Guid.NewGuid(), Name = "Science" };
            #endregion

            #region Taglists
            TagList tagList1 = new TagList { PostsId = blogPost1.Id, Tags = new List<Tag> { tag1, tag2 } };
            TagList tagList2= new TagList { PostsId = project1.Id, Tags = new List<Tag> { tag1, tag2 } };

            taglists.Add(tagList1);
            taglists.Add(tagList2);
            #endregion
            
            Assert.NotNull(taglists);

            Assert.Equal("Alice Johnson", person1.Fullname);

            Assert.Equal("Alice's Blog", blogPost1.Title);

            Assert.Equal("Alice's Project", project1.Title);

            Assert.Equal("Technology", tag1.Name);
            Assert.Equal("Science", tag2.Name);

            Assert.Equal(2, tagList1.Tags.Count);
            Assert.Equal(2, tagList2.Tags.Count);
        }
    }
}
