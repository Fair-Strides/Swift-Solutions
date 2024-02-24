using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.DAL.Concrete
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly DbSet<Tag> _tags;
        private readonly PopNGoDB _context;
        public TagRepository(PopNGoDB context) : base(context)
        {
            _tags = context.Tags;
            _context = context;
        }

        public async Task<Tag> FindByName(string name)
        {
            return await _tags.Where(u => u.Name == name).FirstOrDefaultAsync();
        }

        public Tag CreateNew(string name)
        {
            Tag tag = new()
            {
                Name = name
            };
            _tags.Add(tag);
            _context.SaveChanges();
            return tag;
        }
    }
}
