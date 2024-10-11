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
    public class TagMethod : ITag
    {
        private readonly TagRepo _tag;
        public TagMethod()
        {
            _tag = new TagRepo();
        }

        public void AddTagsToPost(Guid postId, List<Tag> tags)
            => _tag.AddTagsToPost(postId, tags);
        public List<Tag> GetTagsForPost(Guid postId)
            => _tag.GetTagsForPost(postId);
        public List<Tag> GetAllTags()
            => _tag.GetAllTags();
        public void DeleteTag(Guid id)
            => _tag.DeleteTag(id);
        public void UpdateTag(Guid id, string newName)
            => _tag.UpdateTag(id,newName);
        public Tag CreateTag(Guid id, string name)
            => _tag.CreateTag(id, name);
        public List<Tag> GetTagsByIds(List<Guid> tagIds)
            => _tag.GetTagsByIds(tagIds);
    }
}
