using MessageBox.Domain.Entities.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MKopaMessageBox.Domain.Entities
{
    [Table(nameof(AppActivityLog))]
    public class AppActivityLog: CommonProperties
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(250)]
        public string ChannelName { get; set; }

        [Required]
        public int ChannelId { get; set; }

        [Required]
        public string MessageData { get; set; }

        [Required]
        [StringLength(500)]
        public string Identifier { get; set; }

        [Required]
        [StringLength(250)]
        public string Operation { get; set; }

        [StringLength(500)]
        public string Data { get; set; }
        public bool? IsSuccessfulOperation { get; set; } = true;

        #region Navigation properties

        #endregion

    }

}
