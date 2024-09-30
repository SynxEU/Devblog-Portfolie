using Devblog.Domain.Model;
using Devblog.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Unitest
{
    public class ModelTestPersons
    {
        [Fact]
        public void UnixTestPerson()
        {
            List<Person> persons = new List<Person>();

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
    }
}
