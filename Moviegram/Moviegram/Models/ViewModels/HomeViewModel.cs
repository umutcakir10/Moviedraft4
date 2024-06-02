using Moviegram.Models.Domain;

namespace Moviegram.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<MoviePost> MoviePosts { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
