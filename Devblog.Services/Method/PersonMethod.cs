using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devblog.Services.Interface;
using Devblog.Domain.Model;
using Devblog.Domain.Repo;

namespace Devblog.Services.Method
{
    public class PersonMethod : IPerson
    {
        private readonly PersonRepo _person;
        private readonly string _testCsvFilePath;

        public PersonMethod(string csvFilePath)
        {
            _person = new PersonRepo(csvFilePath);
        }

        public PersonMethod() : this("C:\\Desktop\\persons.csv") { }

        public Person CreatePerson(Guid id, string firstName, string lastName, int age, string email, string password, string city, string phoneNumber, string linkedIn, string github)
            => _person.CreatePerson(id, firstName, lastName, age, email, password, city, phoneNumber, linkedIn, github);
        public Person Login(string email, string password)
            => _person.Login(email, password);
        public void UpdatePerson(Guid id, string newFirstName = null, string newLastName = null, int? newAge = null, string newPassword = null, string newCity = null, string newPhoneNumber = null, string newLinkedIn = null, string newGithub = null)
            => _person.UpdatePerson(id, newFirstName, newLastName, newAge, newPassword, newCity, newPhoneNumber, newLinkedIn, newGithub);
        public void DeletePerson(Guid id)
            => _person.DeletePerson(id);
    }
}
