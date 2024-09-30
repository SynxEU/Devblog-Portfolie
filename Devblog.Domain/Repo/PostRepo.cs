using Devblog.Domain.Model;

namespace Devblog.Domain.Repo
{
    public class PostRepo
    {
        private readonly TagRepo _tagRepo;
        private readonly string _csvFilePath = "posts.csv";

        public PostRepo()
        {
            _tagRepo = new TagRepo();

            if (!File.Exists(_csvFilePath))
            {
                File.WriteAllText(_csvFilePath, "Id,Title,AuthorFullName,AuthorEmail,Reference,Type,Content,IsDeleted\n");
            }
        }

        public Post CreatePost(Guid id, string title, Person author, string reference, bool type, string content, List<Tag> tags)
        {
            bool isDeleted = false;
            Post newPost;

            if (id == Guid.Empty) id = Guid.NewGuid();

            if (type)
            {
                newPost = new BlogPost(id, title, author, reference, content, isDeleted);
            }
            else
            {
                newPost = new Project(id, title, author, reference, content, isDeleted);
            }

            string csvLine = $"{newPost.Id},{newPost.Title},{newPost.Author.Fullname},{newPost.Author.Email},{newPost.Reference},{(type ? "Blog" : "Project")},{content},{newPost.IsDeleted}";
            File.AppendAllText(_csvFilePath, csvLine + Environment.NewLine);

            if (tags != null && tags.Count > 0)
            {
                _tagRepo.AddTagsToPost(id, tags);
            }

            return newPost;
        }

        public void DeletePost(Guid id)
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

        public void UpdatePost(Guid id, string newTitle, string newReference, string newContent)
        {
            List<string> lines = File.ReadAllLines(_csvFilePath).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == id.ToString())
                {
                    if (newTitle != null) fields[1] = newTitle;
                    if (newReference != null) fields[4] = newReference;
                    if (newContent != null) fields[6] = newContent;

                    lines[i] = string.Join(",", fields);
                    break;
                }
            }
            File.WriteAllLines(_csvFilePath, lines);
        }

        public void SoftDeletePost(Guid id)
        {
            List<string> lines = File.ReadAllLines(_csvFilePath).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == id.ToString())
                {
                    fields[7] = "True";
                    lines[i] = string.Join(",", fields);
                    break;
                }
            }
            File.WriteAllLines(_csvFilePath, lines);
        }

        public List<Post> GetAllPosts(bool includeDeleted)
        {
            IEnumerable<string> lines = File.ReadAllLines(_csvFilePath).Skip(1);
            List<Post> posts = new List<Post>();

            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                Guid id = Guid.Parse(fields[0]);
                string title = fields[1];
                Person author = new Person
                {
                    FirstName = fields[2].Split(' ')[0],
                    LastName = fields[2].Split(' ')[1],
                    Email = fields[3]
                };
                string reference = fields[4];
                bool type = fields[5] == "Blog";
                string content = fields[6];
                bool isDeleted = bool.Parse(fields[7]);

                if (includeDeleted || !isDeleted)
                {
                    Post post;
                    if (type)
                    {
                        post = new BlogPost(id, title, author, reference, content, isDeleted);
                    }
                    else
                    {
                        post = new Project(id, title, author, reference, content, isDeleted);
                    }
                    posts.Add(post);
                }
            }

            return posts;
        }
    }
}
