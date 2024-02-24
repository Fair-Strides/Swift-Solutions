using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Task<Tag> FindByName(string name);

        public Tag CreateNew(string name);
    }
}
