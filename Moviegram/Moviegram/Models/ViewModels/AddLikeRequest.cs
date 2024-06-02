using System;

namespace Moviegram.Models.ViewModels
{
    public class AddLikeRequest
    {
        public Guid MoviePostId { get; set; }

        public Guid UserId { get; set; }
    }
}
