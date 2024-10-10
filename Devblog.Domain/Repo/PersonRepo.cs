using Devblog.Domain.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Devblog.Domain.Repo
{
    public class PersonRepo
    {
        private readonly Sql _sql;

        public PersonRepo()
        {
            _sql = new Sql();
        }

        public Person CreatePerson(string firstName, string lastName, int age, string email, string password, string city, string phoneNumber, string linkedIn, string github)
        {
            SqlCommand cmd = _sql.Execute("sp_CreateUser");

            cmd.Parameters.AddWithValue("@FName", firstName);
            cmd.Parameters.AddWithValue("@LName", lastName);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@Mail", email);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@LinkedIn", linkedIn);
            cmd.Parameters.AddWithValue("@Github", github);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                return new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Email = email,
                    Password = password,
                    City = city,
                    PhoneNumber = phoneNumber,
                    LinkedIn = linkedIn,
                    Github = github
                };
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public Person Login(string email, string password)
        {
            SqlCommand cmd = _sql.Execute("sp_UserLogOn");
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            try
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Person
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Email = reader.GetString(4),
                            Password = reader.GetString(5),
                            City = reader.GetString(6),
                            PhoneNumber = reader.GetString(7),
                            LinkedIn = reader.GetString(8),
                            Github = reader.GetString(9)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return null;
        }

        public void UpdatePersonPassword(Guid id, string newPassword)
        {
            SqlCommand cmd = _sql.Execute("sp_UpdatePersonPassword");
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Password", newPassword);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public void UpdatePerson(Guid id, string newFirstName, string newLastName, int? newAge, string newCity, string newPhoneNumber, string newLinkedIn, string newGithub)
        {
            SqlCommand cmd = _sql.Execute("sp_UpdatePerson");

            cmd.Parameters.AddWithValue("@PersonId", id);
            cmd.Parameters.AddWithValue("@FName", newFirstName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LName", newLastName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Age", newAge.HasValue ? newAge.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@City", newCity ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Number", newPhoneNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LinkedIn", newLinkedIn ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Github", newGithub ?? (object)DBNull.Value);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public void DeletePerson(Guid id)
        {
            SqlCommand cmd = _sql.Execute("sp_DeletePerson");
            cmd.Parameters.AddWithValue("@PersonId", id);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public Person GetPersonById(Guid id)
        {
            SqlCommand cmd = _sql.Execute("sp_GetUserById");
            cmd.Parameters.AddWithValue("@PersonID", id);

            try
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Person
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Email = reader.GetString(4),
                            Password = reader.GetString(5),
                            City = reader.GetString(6),
                            PhoneNumber = reader.GetString(7),
                            LinkedIn = reader.GetString(8),
                            Github = reader.GetString(9)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return null;
        }

        public List<Person> GetAllUsers()
        {
            SqlCommand cmd = _sql.Execute("sp_GetAllUsers");
            List<Person> users = new List<Person>();

            try
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new Person
                        {
                            Id = reader.GetGuid(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Email = reader.GetString(4),
                            Password = reader.GetString(5),
                            City = reader.GetString(6),
                            PhoneNumber = reader.GetString(7),
                            LinkedIn = reader.GetString(8),
                            Github = reader.GetString(9)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return users;
        }

    }
}
