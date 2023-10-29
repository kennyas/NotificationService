using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MKopaMessageBox.Domain.DTOs
{
    public class EmailModelDTO
    {
        [Required]
        public string ReceiverEmail { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        [Required]
        public string EmailBody { get; set; }

        public string Name { get; set; }
        [Required]
        public bool IsTransactional { get; set; }

        public string[] CarbonCopy { get; set; }
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

    public class SMSRequest
    {
        public string ReceiverNumber { get; set; }
        public string SMSMessage { get; set; }
        public string ApplicationId { get; set; }
        public string Priority { get; set; }
    }

    public class SMSResponse
    {
        public string response_code { get; set; }
        public string response_desc { get; set; }
        public string response_message { get; set; }
        public string applicationId { get; set; }
        public string receiverNumber { get; set; }
    }
}
