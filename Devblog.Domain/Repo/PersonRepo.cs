using Devblog.Domain.Model;

namespace Devblog.Domain.Repo
{
    public class PersonRepo
    {
        private readonly string _csvFilePath;

        public PersonRepo(string csvFilePath = "C:\\Desktop\\persons.csv")
        {
            _csvFilePath = csvFilePath;

            if (!File.Exists(_csvFilePath))
            {
                File.WriteAllText(_csvFilePath, "Id,FirstName,LastName,Age,Email,Password,City,PhoneNumber,LinkedIn,Github\n");
            }
        }

        public Person CreatePerson(Guid id, string firstName, string lastName, int age, string email, string password, string city, string phoneNumber, string linkedIn, string github)
        {
            if (id == Guid.Empty) id = Guid.NewGuid();

            Person newPerson = new Person
            {
                Id = id,
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

            string csvLine = $"{newPerson.Id},{newPerson.FirstName},{newPerson.LastName},{newPerson.Age},{newPerson.Email},{newPerson.Password},{newPerson.City},{newPerson.PhoneNumber},{newPerson.LinkedIn},{newPerson.Github}";
            File.AppendAllText(_csvFilePath, csvLine + Environment.NewLine);

            return newPerson;
        }

        public Person Login(string email, string password)
        {
            List<string> lines = File.ReadAllLines(_csvFilePath).ToList();

            foreach (string line in lines.Skip(1))
            {
                string[] fields = line.Split(',');

                if (fields[4] == email && fields[5] == password)
                {
                    return new Person
                    {
                        Id = Guid.Parse(fields[0]),
                        FirstName = fields[1],
                        LastName = fields[2],
                        Age = int.Parse(fields[3]),
                        Email = fields[4],
                        Password = fields[5],
                        City = fields[6],
                        PhoneNumber = fields[7],
                        LinkedIn = fields[8],
                        Github = fields[9]
                    };
                }
            }

            return null;
        }

        public void UpdatePerson(Guid id, string newFirstName, string newLastName, int? newAge, string newPassword, string newCity, string newPhoneNumber, string newLinkedIn, string newGithub)
        {
            List<string> lines = File.ReadAllLines(_csvFilePath).ToList();

            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == id.ToString())
                {
                    if (newFirstName != null) fields[1] = newFirstName;
                    if (newLastName != null) fields[2] = newLastName;
                    if (newAge.HasValue) fields[3] = newAge.Value.ToString();
                    if (newPassword != null) fields[5] = newPassword;
                    if (newCity != null) fields[6] = newCity;
                    if (newPhoneNumber != null) fields[7] = newPhoneNumber;
                    if (newLinkedIn != null) fields[8] = newLinkedIn;
                    if (newGithub != null) fields[9] = newGithub;

                    lines[i] = string.Join(",", fields);
                    break;
                }
            }

            File.WriteAllLines(_csvFilePath, lines);
        }

        public void DeletePerson(Guid id)
        {
            List<string> lines = File.ReadAllLines(_csvFilePath).ToList();

            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == id.ToString())
                {
                    lines.RemoveAt(i);
                    break;
                }
            }

            File.WriteAllLines(_csvFilePath, lines);
        }
    }
}

