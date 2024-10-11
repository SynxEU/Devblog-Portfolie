using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devblog.Domain.Model;

namespace Devblog.Services.Interface
{
    public interface IPerson
    {
        Person CreatePerson(string firstName, string lastName, int age, string email, string password, string city, string phoneNumber, string linkedIn, string github);
        Person Login(string email, string password);
        void UpdatePersonPassword(Guid id, string newPassword);
        void UpdatePerson(Guid id, string newFirstName, string newLastName, int? newAge, string newCity, string newPhoneNumber, string newLinkedIn, string newGithub);
        void DeletePerson(Guid id);
        Person GetPersonById(Guid id);
        List<Person> GetAllUsers();
        Person GetPersonByMail(string mail);
    }
}