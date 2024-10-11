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

        public PersonMethod()
        {
            _person = new PersonRepo();
        }

        public Person CreatePerson(string firstName, string lastName, int age, string email, string password, string city, string phoneNumber, string linkedIn, string github)
            => _person.CreatePerson(firstName, lastName, age, email, password, city, phoneNumber, linkedIn, github);
        public Person Login(string email, string password)
            => _person.Login(email, password);
        public void UpdatePersonPassword(Guid id, string newPassword)
            => _person.UpdatePersonPassword(id, newPassword);
        public void UpdatePerson(Guid id, string newFirstName, string newLastName, int? newAge, string newCity, string newPhoneNumber, string newLinkedIn, string newGithub)
            => _person.UpdatePerson(id, newFirstName, newLastName, newAge, newCity, newPhoneNumber, newLinkedIn, newGithub);
        public void DeletePerson(Guid id)
            => _person.DeletePerson(id);
        public Person GetPersonById(Guid id)
            => _person.GetPersonById(id);
        public List<Person> GetAllUsers()
            => _person.GetAllUsers();
        public Person GetPersonByMail(string mail)
            => _person.GetPersonByMail(mail);
    }
}
