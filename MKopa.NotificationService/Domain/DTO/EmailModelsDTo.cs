namespace MKopaMessageBox.Domain.DTO
{
    public class EmailModelsDTo
    {
        public string appId { get; set; }
        public string appReference { get; set; }
        public string senderName { get; set; }
        public string senderEmail { get; set; }
        public string recipientEmails { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public Attachment[] attachments { get; set; }
        public bool priority { get; set; }
    }
    public class Attachment
    {
        public string fileName { get; set; }
        public string base64string { get; set; }
    }


    public class EmailSentResponseDto
    {
        public bool status { get; set; }
        public string message { get; set; }
    }


}
