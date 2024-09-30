using Devblog.Domain.Model;
using Devblog.Domain.Repo;
using Devblog.Services.Method;

namespace Devblog.Tests
{
    public class PersonRepoTests : IDisposable
    {
        private readonly string _testCsvFilePath;
        private readonly PersonMethod _personMethod;

        public PersonRepoTests()
        {
            _testCsvFilePath = Path.GetTempFileName();
            File.WriteAllText(_testCsvFilePath, "Id,FirstName,LastName,Age,Email,Password,City,PhoneNumber,LinkedIn,Github\n");
            _personMethod = new PersonMethod(_testCsvFilePath);
        }

        [Fact]
        public void CreatePersonTest()
        {
            Guid id = Guid.NewGuid();
            string firstName = "John";
            string lastName = "Doe";
            int age = 30;
            string email = "john.doe@example.com";
            string password = "password123";
            string city = "New York";
            string phoneNumber = "1234567890";
            string linkedIn = "john.doe";
            string github = "johndoe";

            Person person = _personMethod.CreatePerson(id, firstName, lastName, age, email.ToLower(), password, city, phoneNumber, linkedIn, github);

            List<string> lines = File.ReadAllLines(_testCsvFilePath).ToList();
            Assert.Equal(2, lines.Count());

            string expectedLine = $"{id},{firstName},{lastName},{age},{email},{password},{city},{phoneNumber},{linkedIn},{github}";
            Assert.Contains(expectedLine, lines[1]);
        }

        [Fact]
        public void LoginTest()
        {
            string email = "jane.doe@example.com";
            string password = "securepassword";
            _personMethod.CreatePerson(Guid.NewGuid(), "Jane", "Doe", 25, email, password, "Los Angeles", "0987654321", "jane.doe", "janedoe");

            Person person = _personMethod.Login(email, password);

            Assert.NotNull(person);
            Assert.Equal(email.ToLower(), person.Email.ToLower());
            Assert.Equal(password, person.Password);
        }

        [Fact]
        public void UpdatePersonTest()
        {
            Guid id = Guid.NewGuid();
            _personMethod.CreatePerson(id, "Mark", "Twain", 40, "mark.twain@example.com", "password123", "Chicago", "1231231234", "mark.twain", "marktwain");

            string newFirstName = "Samuel";
            string newLastName = "Clemens";
            int newAge = 45;
            string newPassword = "newpassword";
            string newCity = "Hannibal";
            string newPhoneNumber = "3213213210";
            string newLinkedIn = "samuel.clemens";
            string newGithub = "samclemens";
            _personMethod.UpdatePerson(id, newFirstName, newLastName, newAge, newPassword, newCity, newPhoneNumber, newLinkedIn, newGithub);


            List<string> lines = File.ReadAllLines(_testCsvFilePath).ToList();
            string[] fields = lines[1].Split(',');

            Assert.Equal(newFirstName, fields[1]);
            Assert.Equal(newLastName, fields[2]);
            Assert.Equal(newAge.ToString(), fields[3]);
            Assert.Equal(newPassword, fields[5]);
            Assert.Equal(newCity, fields[6]);
            Assert.Equal(newPhoneNumber, fields[7]);
            Assert.Equal(newLinkedIn, fields[8]);
            Assert.Equal(newGithub, fields[9]);
        }

        [Fact]
        public void DeletePersonTest()
        {
            Guid id = Guid.NewGuid();
            _personMethod.CreatePerson(id, "Alice", "Smith", 35, "alice.smith@example.com", "mypassword", "San Francisco", "9876543210", "alice.smith", "alicesmith");

            _personMethod.DeletePerson(id);

            List<string> lines = File.ReadAllLines(_testCsvFilePath).ToList();
            Assert.Single(lines);
        }

        public void Dispose()
        {
            if (File.Exists(_testCsvFilePath))
            {
                File.Delete(_testCsvFilePath);
            }
        }
    }
}
