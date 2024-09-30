using Devblog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog.Services.Interface
{
    public interface ITag
    {
        void AddTagsToPost(Guid postId, List<Tag> tags);
        List<Tag> GetTagsForPost(Guid postId);
        List<Tag> GetAllTags();
        void DeleteTag(Guid id);
        void UpdateTag(Guid id, string newName);
        Tag CreateTag(Guid id, string name);
    }
}
