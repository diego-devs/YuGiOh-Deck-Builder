using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YGODeckBuilder.Data.EntityModels
{
    [Table("FavoriteCards")]
    public class FavoriteCard
    {
        [Key]
        public int FavoriteId { get; set; }

        [Required]
        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserAccount User { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
