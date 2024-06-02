using Microsoft.EntityFrameworkCore;
using Moviegram.Data;
using Moviegram.Models.Domain;

namespace Moviegram.Repositories
{
    public class MoviePostRepository : IMoviePostRepository
    {
        private readonly MoviegramDbContext moviegramDbContext;

        public MoviePostRepository(MoviegramDbContext moviegramDbContext)
        {
            this.moviegramDbContext = moviegramDbContext;
        }
        public async Task<MoviePost> AddAsync(MoviePost moviePost)
        {
            await moviegramDbContext.AddAsync(moviePost);
            await moviegramDbContext.SaveChangesAsync();
            return moviePost;
        }

        public async Task<MoviePost?> DeleteAsync(Guid id)
        {
            var existingMovie = await moviegramDbContext.MoviePosts.FindAsync(id);

            if (existingMovie != null)
            {
                moviegramDbContext.MoviePosts.Remove(existingMovie);
                await moviegramDbContext.SaveChangesAsync();
                return existingMovie;
            }
            return null;
        }

        public async Task<IEnumerable<MoviePost>> GetAllAsync()
        {
            return await moviegramDbContext.MoviePosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<MoviePost?> GetAsync(Guid id)
        {
            return await moviegramDbContext.MoviePosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MoviePost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await moviegramDbContext.MoviePosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<MoviePost?> UpdateAsync(MoviePost moviePost)
        {
            var existingMovie = await moviegramDbContext.MoviePosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == moviePost.Id);
            if (existingMovie != null) 
            {
                existingMovie.Id = moviePost.Id;
                existingMovie.Heading = moviePost.Heading;
                existingMovie.PageTitle = moviePost.PageTitle;
                existingMovie.Content = moviePost.Content;
                existingMovie.ShortDescription = moviePost.ShortDescription;
                existingMovie.Author = moviePost.Author;
                existingMovie.FeaturedImageUrl = moviePost.FeaturedImageUrl;
                existingMovie.UrlHandle = moviePost.UrlHandle;
                existingMovie.Visible = moviePost.Visible;
                existingMovie.PublishedDate = moviePost.PublishedDate;
                existingMovie.Tags = moviePost.Tags;

                await moviegramDbContext.SaveChangesAsync();
                return existingMovie;
            }

            return null;
        }
    }
}
