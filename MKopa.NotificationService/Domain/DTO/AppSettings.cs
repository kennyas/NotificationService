using MKopa.NotificationService.Utils;

namespace MKopaMessageBox.Domain.DTOs
{
    public class AppSettings
    {
        public string AppName { get; set; }
        public string AppKey { get; set; }
        public bool ActivateMiddleware { get; set; }
        public MessagesExpiryDurationInSeconds MessagesExpiryDurationInSeconds { get; set; }
        public MsgQueue MsgQueue { get; set; }
        public AccessBankService AccessBankService { get; set; }
    }

    public class MessagesExpiryDurationInSeconds
    {
        public int CorpCode { get; set; }
    }

    public class MsgQueue
    {
        public int DelayInMilliseconds { get; set; }
        public bool IsAutoAcknowledged { get; set; }
    }
    public class AccessBankService : ApiServicesCommon {

        public string AppId { get; set; }
        public string AppReference { get; set; }
    }
}
