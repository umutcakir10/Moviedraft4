using Moviegram.Models.Domain;

namespace Moviegram.Repositories
{
    public interface IMoviePostLikeRepository
    {
        Task<int> GetTotalLikes(Guid moviePostId);

        Task<IEnumerable<MoviePostLike>>GetLikesForMovie(Guid moviePostId);

        Task<MoviePostLike> AddLikeForMovie(MoviePostLike moviePostLike);
    }
}
