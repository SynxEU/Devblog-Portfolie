using Devblog.Domain.Model;
using Microsoft.Data.SqlClient;

namespace Devblog.Domain.Repo
{
    public class TagRepo
    {
        private readonly Sql _sql;

        public TagRepo()
        {
            _sql = new Sql();
        }

        public Tag CreateTag(Guid id, string name)
        {
            SqlCommand cmd = _sql.Execute("sp_CreateTag");
            cmd.Parameters.AddWithValue("@TagName", name);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                return new Tag
                {
                    Name = name
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


        public void UpdateTag(Guid id, string newName)
        {
            SqlCommand cmd = _sql.Execute("sp_UpdateTag");
            cmd.Parameters.AddWithValue("@TagID", id);
            cmd.Parameters.AddWithValue("@NewTagName", newName);

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


        public void DeleteTag(Guid id)
        {
            SqlCommand cmd = _sql.Execute("sp_DeleteTag");
            cmd.Parameters.AddWithValue("@TagID", id);

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

        public List<Tag> GetAllTags()
        {
            SqlCommand cmd = _sql.Execute("sp_GetAllTags");

            List<Tag> tags = new List<Tag>();

            try
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tags.Add(new Tag
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1)
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

            return tags;
        }

        public List<Tag> GetTagsByIds(List<Guid> tagIds)
        {
            SqlCommand cmd = _sql.Execute("sp_GetTagById");

            List<Tag> tags = new List<Tag>();

            try
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        foreach (Guid id in tagIds)
                        {
                            tags.Add(new Tag
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1)
                            });
                        }
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

            return tags;
        }

        public List<Tag> GetTagsForPost(Guid postId)
        {
            SqlCommand cmd = _sql.Execute("sp_GetTagsForPost");
            cmd.Parameters.AddWithValue("@PostID", postId);

            List<Tag> tags = new List<Tag>();

            try
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tags.Add(new Tag
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1)
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

            return tags;
        }


        public void AddTagsToPost(Guid postId, List<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                SqlCommand cmd = _sql.Execute("sp_AddTagsToPost");
                cmd.Parameters.AddWithValue("@PostID", postId);
                cmd.Parameters.AddWithValue("@TagID", tag.Id);

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
        }

    }
}
