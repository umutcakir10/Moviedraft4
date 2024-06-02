using Microsoft.AspNetCore.Mvc;
using Moviegram.Models;
using System.Diagnostics;
using Moviegram.Repositories;
using Moviegram.Models.ViewModels;

namespace Moviegram.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IMoviePostRepository moviePostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger,
			IMoviePostRepository moviePostRepository,
			ITagRepository tagRepository
			)
		{
			_logger = logger;
            this.moviePostRepository = moviePostRepository;
            this.tagRepository = tagRepository;
        }

		public async Task<IActionResult> Index()
		{
			// Getting all movies
			var moviePosts = await moviePostRepository.GetAllAsync();
			// Getting all tags
			var tags = await tagRepository.GetAllAsync();

			var model = new HomeViewModel
			{
				MoviePosts = moviePosts,
				Tags = tags
			};

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
