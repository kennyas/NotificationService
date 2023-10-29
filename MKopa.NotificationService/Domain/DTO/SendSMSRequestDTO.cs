using System.ComponentModel.DataAnnotations;

namespace MKopaMessageBox.Domain.DTO
{
    public class SendSMSRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string CustomerId { get; set; }

        [Required]
        public string SMSBody { get; set; }

        [Required]
        public string MSIDN { get; set; }
        [Required]
        public string AccountNumber { get; set; }
    }

    public class SmsAuthRequestDto
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class MessageRequestDTO 
    {
        public string message { get; set; }
        public string sender { get; set; }
        public string message_type { get; set; }
        public string dlr_url { get; set; }
        public List<string> msisdns { get; set; }
        public string message_id { get; set; }
    }

    public class CallbackDto
    {
        public string message_id { get; set; }
        public string status { get; set; }
        public string msisdn { get; set;}
    }


    public class SendSMSRequestNewDTO
    {
        public string appId { get; set; }
        public string phoneNumber { get; set; }
        public string message { get; set; }
        public string appReference { get; set; }
        public bool priority { get; set; }
        public string debitAccount { get; set; }
    }

}
