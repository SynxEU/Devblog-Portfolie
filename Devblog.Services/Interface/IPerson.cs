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
        Person CreatePerson(Guid id, string firstName, string lastName, int age, string email, string password, string city, string phoneNumber, string linkedIn, string github);
        Person Login(string email, string password);
        void UpdatePerson(Guid id, string newFirstName = null, string newLastName = null, int? newAge = null, string newPassword = null, string newCity = null, string newPhoneNumber = null, string newLinkedIn = null, string newGithub = null);
        void DeletePerson(Guid id);
    }
}
