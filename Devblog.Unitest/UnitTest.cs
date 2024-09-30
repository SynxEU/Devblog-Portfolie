using Devblog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Unitest
{
    public class UnitTest
    {
        List<Person> persons = new List<Person>();
        List<Post> posts = new List<Post>();
        List<Tag> tags = new List<Tag>();
        List<TagList> taglists = new List<TagList>();

        [Fact]
        public void UnixTestPerson()
        {
            Person person = new Person
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

            persons.Add(person);

            Assert.NotNull(person);

            Assert.NotEmpty(persons);

            Assert.Equal("Alice Johnson", person.Fullname);
        }

        [Fact]
        public void UnixTestProject()
        {
            #region Person
            Person person = new Person
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

            Post projectPost = new Project(Guid.NewGuid(), "Alice's Project", person, "https://aliceblog.com", "https://aliceblog.com/weblog", false);

            posts.Add(projectPost);

            Assert.NotNull(projectPost);

            Assert.NotEmpty(posts);

            Assert.Equal("Alice's Project", projectPost.Title);
        }

        [Fact]
        public void UnixTestBlog()
        {
            #region Person
            Person person = new Person
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

            Post blogPost = new BlogPost(Guid.NewGuid(), "Alice's Blog", person, "https://aliceblog.com", "https://aliceblog.com/weblog", false);

            posts.Add(blogPost);

            Assert.NotNull(blogPost);

            Assert.NotEmpty(posts);

            Assert.Equal("Alice's Blog", blogPost.Title);
        }

        [Fact]
        public void UnixTestTag()
        {
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

        [Fact]
        public void TagsListUnixTests()
        {
            #region Person
            Person person = new Person
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

            #region Post
            Post projectPost = new Project(Guid.NewGuid(), "Alice's Project", person, "https://aliceblog.com", "https://aliceblog.com/weblog", false);
            Post blogPost = new BlogPost(Guid.NewGuid(), "Alice's Blog", person, "https://aliceblog.com", "https://aliceblog.com/weblog", false);
            #endregion

            #region Tags
            Tag tag1 = new Tag { Id = Guid.NewGuid(), Name = "Technology" };
            Tag tag2 = new Tag { Id = Guid.NewGuid(), Name = "Science" };
            #endregion Tags

            TagList tagList1 = new TagList { PostsId = blogPost.Id, Tags = new List<Tag> { tag1, tag2 } };
            TagList tagList2 = new TagList { PostsId = projectPost.Id, Tags = new List<Tag> { tag1, tag2 } };

            taglists.Add(tagList1);
            taglists.Add(tagList2);

            Assert.NotNull(taglists);

            Assert.Equal(2, tagList1.Tags.Count);
            Assert.Equal(2, tagList2.Tags.Count);
        }
    }
}
