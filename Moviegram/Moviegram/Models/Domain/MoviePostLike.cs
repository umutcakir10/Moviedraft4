namespace Moviegram.Models.Domain
{
    public class MoviePostLike
    {
        public Guid Id { get; set; }

        public Guid MoviePostId { get; set; }

        public Guid UserId { get; set; }
    }
}
