using Devblog.Domain.Model;

namespace Devblog.Domain.Repo
{
    public class TagRepo
    {
        private readonly string _tagsCsvFilePath = "tags.csv";
        private readonly string _tagListCsvFilePath = "tagLists.csv";

        public TagRepo()
        {
            if (!File.Exists(_tagsCsvFilePath))
            {
                File.WriteAllText(_tagsCsvFilePath, "Id,Name\n");
            }
            if (!File.Exists(_tagListCsvFilePath))
            {
                File.WriteAllText(_tagListCsvFilePath, "PostId,TagId\n");
            }
        }

        public Tag CreateTag(Guid id, string name)
        {
            if (id == Guid.Empty) id = Guid.NewGuid();

            Tag newTag = new Tag { Id = id, Name = name };

            string csvLine = $"{newTag.Id},{newTag.Name}";
            File.AppendAllText(_tagsCsvFilePath, csvLine + Environment.NewLine);

            return newTag;
        }

        public void UpdateTag(Guid id, string newName)
        {
            List<string> lines = File.ReadAllLines(_tagsCsvFilePath).ToList();

            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == id.ToString())
                {
                    fields[1] = newName;
                    lines[i] = string.Join(",", fields);
                    break;
                }
            }

            File.WriteAllLines(_tagsCsvFilePath, lines);
        }

        public void DeleteTag(Guid id)
        {
            List<string> lines = File.ReadAllLines(_tagsCsvFilePath).ToList();

            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == id.ToString())
                {
                    lines.RemoveAt(i);
                    break;
                }
            }

            File.WriteAllLines(_tagsCsvFilePath, lines);

            lines = File.ReadAllLines(_tagListCsvFilePath).ToList();
            lines.RemoveAll(line => line.Split(',')[1] == id.ToString());
            File.WriteAllLines(_tagListCsvFilePath, lines);
        }

        public List<Tag> GetAllTags()
        {
            List<string> lines = File.ReadAllLines(_tagsCsvFilePath).Skip(1).ToList();
            List<Tag> tags = new List<Tag>();

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                Tag tag = new Tag
                {
                    Id = Guid.Parse(fields[0]),
                    Name = fields[1]
                };
                tags.Add(tag);
            }

            return tags;
        }

        public List<Tag> GetTagsForPost(Guid postId)
        {
            List<string> lines = File.ReadAllLines(_tagListCsvFilePath).Skip(1).ToList();
            List<Guid> tagIds = lines.Where(line => line.Split(',')[0] == postId.ToString())
                                      .Select(line => Guid.Parse(line.Split(',')[1]))
                                      .ToList();

            List<Tag> tags = GetAllTags();
            return tags.Where(tag => tagIds.Contains(tag.Id)).ToList();
        }

        public void AddTagsToPost(Guid postId, List<Tag> tags)
        {
            List<string> lines = new List<string>();
            foreach (Tag tag in tags)
            {
                lines.Add($"{postId},{tag.Id}");
            }

            File.AppendAllLines(_tagListCsvFilePath, lines);
        }
    }
}
