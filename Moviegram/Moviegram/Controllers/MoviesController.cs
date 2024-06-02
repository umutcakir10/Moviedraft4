using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviegram.Models.Domain;
using Moviegram.Models.ViewModels;
using Moviegram.Repositories;

namespace Moviegram.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviePostRepository moviePostRepository;
        private readonly IMoviePostLikeRepository moviePostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMoviePostCommentRepository moviePostCommentRepository;

        public MoviesController(IMoviePostRepository moviePostRepository,
            IMoviePostLikeRepository moviePostLikeRepository,
            SignInManager<IdentityUser>signInManager,
            UserManager<IdentityUser>userManager,
            IMoviePostCommentRepository moviePostCommentRepository)
        {
            this.moviePostRepository = moviePostRepository;
            this.moviePostLikeRepository = moviePostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.moviePostCommentRepository = moviePostCommentRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var moviePost = await moviePostRepository.GetByUrlHandleAsync(urlHandle);
            var movieDetailsViewModel = new MovieDetailsViewModel();



            if ( moviePost != null)
            {
                
                var totalLikes = await moviePostLikeRepository.GetTotalLikes(moviePost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    var likesForMovie = await moviePostLikeRepository.GetLikesForMovie(moviePost.Id);

                    var userId = userManager.GetUserId(User);

                    if ( userId != null )
                    {
                        var likeFromUser = likesForMovie.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                var movieCommentsDomainModel = await moviePostCommentRepository.GetCommentsByMovieIdAsync(moviePost.Id);

                var movieCommentsForView = new List<MovieComment>();

                foreach (var movieComment in movieCommentsDomainModel)
                {
                    movieCommentsForView.Add(new MovieComment
                    {
                        Description = movieComment.Description,
                        DateAdded = movieComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(movieComment.UserId.ToString())).UserName
                    });
                }

                movieDetailsViewModel = new MovieDetailsViewModel
                {
                    Id = moviePost.Id,
                    Content = moviePost.Content,
                    PageTitle = moviePost.PageTitle,
                    Author = moviePost.Author,
                    FeaturedImageUrl = moviePost.FeaturedImageUrl,
                    Heading = moviePost.Heading,
                    PublishedDate = moviePost.PublishedDate,
                    ShortDescription = moviePost.ShortDescription,
                    UrlHandle = moviePost.UrlHandle,
                    Visible = moviePost.Visible,
                    Tags = moviePost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = movieCommentsForView
                };

            }
            return View(movieDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(MovieDetailsViewModel movieDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new MoviePostComment
                {
                    MoviePostId = movieDetailsViewModel.Id,
                    Description = movieDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await moviePostCommentRepository.AddAsync(domainModel);

                return RedirectToAction("Index", "Home",
                    new { urlHandle = movieDetailsViewModel.UrlHandle });

            }
            return View();
          
        }
    }
}
