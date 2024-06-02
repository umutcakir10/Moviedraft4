using Microsoft.EntityFrameworkCore;
using Moviegram.Data;
using Moviegram.Models.Domain;

namespace Moviegram.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly MoviegramDbContext moviegramDbContext;

        public TagRepository(MoviegramDbContext moviegramDbContext)
        {
            this.moviegramDbContext = moviegramDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await moviegramDbContext.Tags.AddAsync(tag);
            await moviegramDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await moviegramDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                moviegramDbContext.Tags.Remove(existingTag);
                await moviegramDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await moviegramDbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return moviegramDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await moviegramDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await moviegramDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }
    }
}
