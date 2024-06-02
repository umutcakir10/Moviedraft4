using Microsoft.AspNetCore.Mvc;
using Moviegram.Models.ViewModels;
using Moviegram.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

using Moviegram.Models.Domain;

namespace Moviegram.Controllers
{
    public class AdminMoviePostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IMoviePostRepository moviePostRepository;

        public AdminMoviePostsController(ITagRepository tagRepository, IMoviePostRepository moviePostRepository)
        {
            this.tagRepository = tagRepository;
            this.moviePostRepository = moviePostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from repository
            var tags = await tagRepository.GetAllAsync();

            var model = new AddMoviePostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMoviePostRequest addMoviePostRequest)
        {
            var moviePost = new MoviePost
            {
                Heading = addMoviePostRequest.Heading,
                PageTitle = addMoviePostRequest.PageTitle,
                Content = addMoviePostRequest.Content,
                ShortDescription = addMoviePostRequest.ShortDescription,
                FeaturedImageUrl = addMoviePostRequest.FeaturedImageUrl,
                UrlHandle = addMoviePostRequest.UrlHandle,
                PublishedDate = addMoviePostRequest.PublishedDate,
                Author = addMoviePostRequest.Author,
                Visible = addMoviePostRequest.Visible,

            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addMoviePostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            moviePost.Tags = selectedTags;

            await moviePostRepository.AddAsync(moviePost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //Call the repository
            var moviePosts = await moviePostRepository.GetAllAsync();

            return View(moviePosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //we retrieve result from repository here

            var moviePost = await moviePostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (moviePost != null)
            {
                var model = new EditMoviePostRequest
                {
                    Id = moviePost.Id,
                    Heading = moviePost.Heading,
                    PageTitle = moviePost.PageTitle,
                    Content = moviePost.Content,
                    Author = moviePost.Author,
                    FeaturedImageUrl = moviePost.FeaturedImageUrl,
                    UrlHandle = moviePost.UrlHandle,
                    ShortDescription = moviePost.ShortDescription,
                    PublishedDate = moviePost.PublishedDate,
                    Visible = moviePost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = moviePost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);
            }

            

            //pass data to view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMoviePostRequest editMoviePostRequest)
        {

            var moviePostDomainModel = new MoviePost
            {
                Id = editMoviePostRequest.Id,
                Heading = editMoviePostRequest.Heading,
                PageTitle = editMoviePostRequest.PageTitle,
                Content = editMoviePostRequest.Content,
                Author = editMoviePostRequest.Author,
                ShortDescription = editMoviePostRequest.ShortDescription,
                FeaturedImageUrl = editMoviePostRequest.FeaturedImageUrl,
                PublishedDate = editMoviePostRequest.PublishedDate,
                UrlHandle = editMoviePostRequest.UrlHandle,
                Visible = editMoviePostRequest.Visible,
            };

            var selectedTags = new List<Tag>();
            foreach ( var selectedTag in editMoviePostRequest.SelectedTags )
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            moviePostDomainModel.Tags = selectedTags;

            var updatedMovie = await moviePostRepository.UpdateAsync(moviePostDomainModel);

            if (updatedMovie != null)
            {
                //Show success not
                return RedirectToAction("Edit");
            }
            //Show error not
            return RedirectToAction("Edit");


        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditMoviePostRequest editMoviePostRequest)
        {

            var deletedMoviePost = await moviePostRepository.DeleteAsync(editMoviePostRequest.Id);

            if (deletedMoviePost != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editMoviePostRequest.Id });

        }

    }
}
