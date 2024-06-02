using Moviegram.Models.Domain;

namespace Moviegram.Repositories
{
    public interface IMoviePostRepository
    {
        Task<IEnumerable<MoviePost>> GetAllAsync();

        Task<MoviePost?> GetAsync(Guid id);

        Task<MoviePost?> GetByUrlHandleAsync(string urlHandle);

        Task<MoviePost> AddAsync(MoviePost moviePost);

        Task<MoviePost?> UpdateAsync(MoviePost moviePost);

        Task<MoviePost?> DeleteAsync(Guid id);
    }
}
