using Moviegram.Models.Domain;

namespace Moviegram.Repositories
{
    public interface IMoviePostCommentRepository
    {
        Task<MoviePostComment> AddAsync(MoviePostComment moviePostComment);

        Task<IEnumerable<MoviePostComment>> GetCommentsByMovieIdAsync(Guid moviePostId);
    }
}
