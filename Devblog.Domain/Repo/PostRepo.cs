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

        public Post CreatePost(string title, Person authorId, string reference, bool isBlogPost, string content, List<Tag> tags)
        {
            SqlCommand cmd = _sql.Execute("sp_CreatePost");

            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@AuthorId", authorId);
            cmd.Parameters.AddWithValue("@Reference", reference);
            cmd.Parameters.AddWithValue("@Type", isBlogPost ? 1 : 0); 
            cmd.Parameters.AddWithValue("@Content", content);
            string tagsString = tags != null ? string.Join(",", tags.Select(t => t.Name)) : null;
            cmd.Parameters.AddWithValue("@Tags", tagsString ?? (object)DBNull.Value);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                if (isBlogPost)
                {
                    return new BlogPost(Guid.NewGuid(), title, authorId, reference, content, false);
                }
                else
                {
                    return new Project(Guid.NewGuid(), title, authorId, reference, content, false);
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
                        Guid id = reader.GetGuid(0);
                        string title = reader.GetString(1);
                        author.Id = reader.GetGuid(2);
                        string reference = reader.GetString(3);
                        bool isBlogPost = reader.GetBoolean(4);
                        string content = reader.GetString(5);
                        bool isDeleted = reader.GetBoolean(6);

                        Post post;

                        if (!isBlogPost)
                        {
                            post = new Project(id, title, author, reference, content, isDeleted);
                        }
                        else
                        {
                            post = new BlogPost(id, title, author, reference, content, isDeleted);
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
                        Guid postId = reader.GetGuid(0);
                        string title = reader.GetString(1);
                        author.Id = reader.GetGuid(2);
                        string reference = reader.GetString(3);
                        bool isBlogPost = reader.GetBoolean(4);
                        string content = reader.GetString(5);
                        bool deleted = reader.GetBoolean(6);

                        Post post;

                        if (!isBlogPost)
                        {
                            return new Project(postId, title, author, reference, content, deleted);
                        }
                        else
                        {
                            return new BlogPost(postId, title, author, reference, content, deleted);
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
                        Guid id = reader.GetGuid(0);
                        string title = reader.GetString(1);
                        Guid personId = reader.GetGuid(2);
                        string reference = reader.GetString(3);
                        bool type = reader.GetBoolean(4);
                        string content = reader.GetString(5);
                        bool isDeleted = reader.GetBoolean(6);

                        Person author = new Person { Id = personId };
                        Post post = type ?
                            new BlogPost(id, title, author, reference, content, isDeleted) :
                            new Project(id, title, author, reference, content, isDeleted);

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
                        Guid id = reader.GetGuid(0);
                        string title = reader.GetString(1);
                        Guid personId = reader.GetGuid(2);
                        string authorEmail = reader.GetString(4);
                        string reference = reader.GetString(5);
                        bool type = reader.GetBoolean(6);
                        string content = reader.GetString(7);
                        bool isDeleted = reader.GetBoolean(8);

                        Person author = new Person
                        {
                            Id = personId,
                            Email = authorEmail
                        };

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
