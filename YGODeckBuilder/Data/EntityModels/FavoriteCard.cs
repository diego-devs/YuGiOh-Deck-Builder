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

        // Anonymous GUID cookie for now; replace with real UserId when auth is added.
        [Required]
        [MaxLength(128)]
        public string UserId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
