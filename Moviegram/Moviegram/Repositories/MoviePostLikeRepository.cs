
using Microsoft.EntityFrameworkCore;
using Moviegram.Data;
using Moviegram.Models.Domain;

namespace Moviegram.Repositories
{
    public class MoviePostLikeRepository : IMoviePostLikeRepository
    {
        private readonly MoviegramDbContext moviegramDbContext;

        public MoviePostLikeRepository(MoviegramDbContext moviegramDbContext)
        {
            this.moviegramDbContext = moviegramDbContext;
        }

        public async Task<MoviePostLike> AddLikeForMovie(MoviePostLike moviePostLike)
        {
            await moviegramDbContext.MoviePostLike.AddAsync(moviePostLike);
            await moviegramDbContext.SaveChangesAsync();
            return moviePostLike;
        }

        public async Task<IEnumerable<MoviePostLike>> GetLikesForMovie(Guid moviePostId)
        {
            return await moviegramDbContext.MoviePostLike.Where(x => x.MoviePostId == moviePostId)
                .ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid moviePostId)
        {
            return await moviegramDbContext.MoviePostLike
                .CountAsync(x => x.MoviePostId == moviePostId);
        }
    }
}
