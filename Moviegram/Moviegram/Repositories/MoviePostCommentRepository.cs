using Microsoft.EntityFrameworkCore;
using Moviegram.Data;
using Moviegram.Models.Domain;

namespace Moviegram.Repositories
{
    public class MoviePostCommentRepository : IMoviePostCommentRepository
    {
        private readonly MoviegramDbContext moviegramDbContext;

        public MoviePostCommentRepository(MoviegramDbContext moviegramDbContext)
        {
            this.moviegramDbContext = moviegramDbContext;
        }
        public async Task<MoviePostComment> AddAsync(MoviePostComment moviePostComment)
        {
            await moviegramDbContext.MoviePostComment.AddAsync(moviePostComment);
            await moviegramDbContext.SaveChangesAsync();
            return moviePostComment;
        }

        public async Task<IEnumerable<MoviePostComment>> GetCommentsByMovieIdAsync(Guid moviePostId)
        {
            return await moviegramDbContext.MoviePostComment.Where(x => x.MoviePostId == moviePostId)
                .ToListAsync();
        }
    }
}
