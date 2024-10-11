using Devblog.Domain.Model;
using System.Data;
using Microsoft.Data.SqlClient;


namespace Devblog.Domain.Repo
{
    public class PostRepo
    {
        private readonly Sql _sql;

        public PostRepo()
        {
            _sql = new Sql();
        }

        public Post CreatePost(string title, Person authorId, string reference, bool isBlogPost, string content)
        {
            SqlCommand cmd = _sql.Execute("sp_CreatePost");

            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@AuthorId", authorId.Id);
            cmd.Parameters.AddWithValue("@Reference", reference);
            cmd.Parameters.AddWithValue("@Type", isBlogPost ? 1 : 0); 
            cmd.Parameters.AddWithValue("@Content", content);
            //string tagsString = tags != null ? string.Join(",", tags.Select(t => t.Name)) : null;
            //cmd.Parameters.AddWithValue("@Tags", tagsString ?? (object)DBNull.Value);
            DateTime currentDate = DateTime.UtcNow;


            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                if (isBlogPost)
                {
                    return new BlogPost(Guid.NewGuid(), title, authorId, reference, content, false, currentDate.Date, currentDate.Date);
                }
                else
                {
                    return new Project(Guid.NewGuid(), title, authorId, reference, content, false, currentDate.Date, currentDate.Date);
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
        }

        public void DeletePost(Guid id)
        {
            SqlCommand cmd = _sql.Execute("sp_DeletePost");
            cmd.Parameters.AddWithValue("@PostId", id);

            try
            {
                cmd.Connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Deleted");
                }
                else
                {
                    Console.WriteLine("Not Deleted");
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
        }

        public void SoftDeletePost(Guid id)
        {
            SqlCommand cmd = _sql.Execute("sp_SoftDeletePost");
            cmd.Parameters.AddWithValue("@PostId", id);

            try
            {
                cmd.Connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("SoftDeleted");
                }
                else
                {
                    Console.WriteLine("Not SoftDeleted");
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
        }

        public List<Post> GetAllPosts(bool includeDeleted)
        {
            List<Post> posts = new List<Post>();
            Person author = new Person();

            SqlCommand cmd = _sql.Execute("sp_GetAllPosts");
            
            cmd.Parameters.AddWithValue("@IsDeleted", includeDeleted ? (object)DBNull.Value : 0);

            try
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Post post;

                        Guid postId = reader.GetGuid(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        author.Id = reader.GetGuid(reader.GetOrdinal("AuthorId"));
                        string reference = reader.GetString(reader.GetOrdinal("Reference"));
                        bool isBlogPost = reader.GetBoolean(reader.GetOrdinal("Type"));
                        string content = reader.GetString(reader.GetOrdinal("Content"));
                        bool deleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                        DateTime createdDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                        DateTime lastUpdate = reader.GetDateTime(reader.GetOrdinal("LastUpdated"));

                        if (!isBlogPost)
                        {
                            post = new Project(postId, title, author, reference, content, deleted, createdDate, lastUpdate);
                        }
                        else
                        {
                            post = new BlogPost(postId, title, author, reference, content, deleted, createdDate, lastUpdate);
                        }

                        posts.Add(post);
                    }
                }

                return posts;
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

        public Post GetPostById(Guid id, bool? isDeleted = null)
        {
            Person author = new Person();

            SqlCommand cmd = _sql.Execute("sp_GetPostByIdAndDeletion");
            cmd.Parameters.AddWithValue("@PostID", id);
            cmd.Parameters.AddWithValue("@IsDeleted", isDeleted.HasValue ? isDeleted.Value : (object)DBNull.Value);

            try
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Guid postId = reader.GetGuid(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        author.Id = reader.GetGuid(reader.GetOrdinal("AuthorId"));
                        string reference = reader.GetString(reader.GetOrdinal("Reference"));
                        bool isBlogPost = reader.GetBoolean(reader.GetOrdinal("Type"));
                        string content = reader.GetString(reader.GetOrdinal("Content"));
                        bool deleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                        DateTime createdDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                        DateTime lastUpdate = reader.GetDateTime(reader.GetOrdinal("LastUpdated"));

                        if (!isBlogPost)
                        {
                            return new Project(postId, title, author, reference, content, deleted, createdDate, lastUpdate);
                        }
                        else
                        {
                            return new BlogPost(postId, title, author, reference, content, deleted, createdDate, lastUpdate);
                        }
                    }
                    else
                    {
                        return null;
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
        }

        public void UpdatePost(Guid id, string newTitle, string newContent, string newReference)
        {
            SqlCommand cmd = _sql.Execute("sp_UpdatePost");

            cmd.Parameters.AddWithValue("@PostID", id);
            cmd.Parameters.AddWithValue("@Title", newTitle);
            cmd.Parameters.AddWithValue("@Content", newContent);
            cmd.Parameters.AddWithValue("@Reference", newReference);

            try
            {
                cmd.Connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Updated");
                }
                else
                {
                    Console.WriteLine("Not Updated");
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
        }

        public void UpdatePostTags(Guid postId, List<Tag> tags)
        {
            SqlCommand cmd = _sql.Execute("sp_UpdatePostTags");

            string tagsString = tags != null ? string.Join(",", tags.Select(t => t.Name)) : null;
            cmd.Parameters.AddWithValue("@PostId", postId);
            cmd.Parameters.AddWithValue("@Tags", tagsString ?? (object)DBNull.Value);

            try
            {
                cmd.Connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Tags updated");
                }
                else
                {
                    Console.WriteLine("Tags not updated");
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
        }

        public List<Post> GetPostsByTag(string tagName)
        {
            List<Post> posts = new List<Post>();
            SqlCommand cmd = _sql.Execute("sp_GetPostsByTag");

            cmd.Parameters.AddWithValue("@TagName", tagName);

            try
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid id = reader.GetGuid(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        Guid personId = reader.GetGuid(reader.GetOrdinal("AuthorId"));
                        string reference = reader.GetString(reader.GetOrdinal("Reference"));
                        bool type = reader.GetBoolean(reader.GetOrdinal("Type"));
                        string content = reader.GetString(reader.GetOrdinal("Content"));
                        bool isDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                        DateTime createdDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                        DateTime lastUpdate = reader.GetDateTime(reader.GetOrdinal("LastUpdated"));

                        Person author = new Person { Id = personId };
                        Post post = type ?
                            new BlogPost(id, title, author, reference, content, isDeleted, createdDate, lastUpdate) :
                            new Project(id, title, author, reference, content, isDeleted, createdDate, lastUpdate);

                        posts.Add(post);
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

            return posts;
        }

        public void RestoreDeletedPost(Guid id)
        {
            SqlCommand cmd = _sql.Execute("sp_RestoreDeletedPost");

            cmd.Parameters.AddWithValue("@PostId", id);

            try
            {
                cmd.Connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Post restored");
                }
                else
                {
                    Console.WriteLine("Post not restored");
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
        }

        public List<Post> GetPostsByAuthorEmail(string email)
        {
            List<Post> posts = new List<Post>();

            SqlCommand cmd = _sql.Execute("sp_GetPostsByAuthorEmail");
            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid id = reader.GetGuid(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        Guid personId = reader.GetGuid(reader.GetOrdinal("AuthorId"));
                        string reference = reader.GetString(reader.GetOrdinal("Reference"));
                        bool type = reader.GetBoolean(reader.GetOrdinal("Type"));
                        string content = reader.GetString(reader.GetOrdinal("Content"));
                        bool isDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                        DateTime createdDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                        DateTime lastUpdate = reader.GetDateTime(reader.GetOrdinal("LastUpdated"));

                        Person author = new Person
                        {
                            Id = personId
                        };

                        Post post;
                        if (type)
                        {
                            post = new BlogPost(id, title, author, reference, content, isDeleted, createdDate, lastUpdate);
                        }
                        else
                        {
                            post = new Project(id, title, author, reference, content, isDeleted, createdDate, lastUpdate);
                        }

                        posts.Add(post);
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

            return posts;
        }

        public List<Post> GetPostsByAuthorId(Guid persId)
        {
            List<Post> posts = new List<Post>();

            SqlCommand cmd = _sql.Execute("sp_GetPostsByAuthorId");
            cmd.Parameters.AddWithValue("@Id", persId);

            try
            {
                cmd.Connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid id = reader.GetGuid(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        Guid personId = persId;
                        string reference = reader.GetString(reader.GetOrdinal("Reference"));
                        bool type = reader.GetBoolean(reader.GetOrdinal("Type"));
                        string content = reader.GetString(reader.GetOrdinal("Content"));
                        bool isDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                        DateTime createdDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                        DateTime lastUpdate = reader.GetDateTime(reader.GetOrdinal("LastUpdated"));

                        Person author = new Person
                        {
                            Id = personId
                        };

                        Post post;
                        if (type)
                        {
                            post = new BlogPost(id, title, author, reference, content, isDeleted, createdDate, lastUpdate);
                        }
                        else
                        {
                            post = new Project(id, title, author, reference, content, isDeleted, createdDate, lastUpdate);
                        }

                        posts.Add(post);
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

            return posts;
        }
    }
}
