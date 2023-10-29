using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MKopaMessageBox.Domain.Entities
{
    [Table(nameof(AccessKey))]
    public class AccessKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string ChannelName { get; set; }

        [Required]
        [StringLength(250)]
        public string KeyName { get; set; }

        [Required]
        [StringLength(250)]
        public string KeyValue { get; set; }

        [StringLength(250)]
        public string Password { get; set; }

        [Required]
        public bool? IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    }
}
