using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moviegram.Models.Domain;
using Moviegram.Models.ViewModels;
using Moviegram.Repositories;

namespace Moviegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviePostLikeController : ControllerBase
    {
        private readonly IMoviePostLikeRepository moviePostLikeRepository;

        public MoviePostLikeController(IMoviePostLikeRepository moviePostLikeRepository)
        {
            this.moviePostLikeRepository = moviePostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new MoviePostLike
            {
                MoviePostId = addLikeRequest.MoviePostId,
                UserId = addLikeRequest.UserId
            };

            await moviePostLikeRepository.AddLikeForMovie(model);

            return Ok(model);
        }

        [HttpGet]
        [Route("{moviePostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForMovie([FromRoute] Guid moviePostId)
        {
            var totalLikes = await moviePostLikeRepository.GetTotalLikes(moviePostId);

            return Ok(totalLikes);
        }
    }
}
