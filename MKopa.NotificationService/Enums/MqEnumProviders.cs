namespace MKopa.NotificationService.Enums
{
    public enum ClientProvidedNameEnum
    {
        None = 0,
        MkopaMsgBoxSvc = 1,
        MkopaLoanSvc = 2,
    }


    public enum ExchangeNameEnum
    {
        None = 0,
        MkopaMsgBoxSvc_Exchange = 1,
        MKopaLoanSvc_Exchange = 2
    }


    public enum QueueNameOrRouteKeyEnums
    {
        None = 0,
        SmsMessages = 1,

        GeneralSMSKey = 2,

    }
}
