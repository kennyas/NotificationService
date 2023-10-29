using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MKopaMessageBox.Domain.Entities
{
    [Table(nameof(MessageBox))]
    public class MessageBox
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string AppName { get; set; }
        public string Phonenumber { get; set; }

        [Required]
        public string Operation { get; set; }
        /// <summary>
        /// Email || SMS || WhatsApp || ... This defaults to Email
        /// </summary>
        public string Channel { get; set; } = "Email";
        public string Description { get; set; }

        public string MessageData { get; set; }

        [Required]
        public string EmailReceiver { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiredAt { get; set; } = DateTime.UtcNow.AddDays(1);

        public string UserId { get; set; }

        public bool IsUsed { get; set; } = false;

        [Required]
        public bool ForQueue { get; set; } = false;

        [Required]
        public bool IsProcessed { get; set; } = false;
    }

}
