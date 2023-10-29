namespace MKopaMessageBox.Domain.DTO
{
    public class SmsResponseDto<T>
    {
        public string message { get; set; }
        public T data { get; set; }
        public int status { get; set; } 
    }

    public class AuthDto
    {
        public string token { get; set; }
        public int expires_at { get; set; }
    }

    public class MessageDto
    {
        public int accepted_successful { get; set; }
        public int your_payload_had { get; set; }
        public string message_id { get; set; }
        public List<string> unroutable_numbers { get; set; }
        public List<string> failed_due_to_insufficient_balance { get; set; }
        public List<string> duplicate_numbers { get; set; }
        public List<string> invalid_msisdns { get; set; }
    }

    public class BalanceDto
    {
        public string currency_code { get; set; }
        public string balance { get; set; }
    }
}
