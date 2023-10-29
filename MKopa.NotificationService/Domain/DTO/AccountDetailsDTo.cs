namespace MKopaMessageBox.Domain.DTO
{
    public class AccountDetailsDTo
    {
        public string status { get; set; }
        public string message { get; set; }
        public Datum[] data { get; set; }
    }
    public class Datum
    {
        public string accountNumber { get; set; }
        public string alternateAccount { get; set; }
        public string accountName { get; set; }
        public string accountType { get; set; }
        public string productCode { get; set; }
        public string productCodeDesc { get; set; }
        public string currencyCode { get; set; }
        public string currency { get; set; }
        public string accountStatus { get; set; }
        public string bvn { get; set; }
        public decimal availableBalance { get; set; }
        public decimal blockedBalance { get; set; }
        public string accountOpenDate { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public string customerId { get; set; }
        public string branchName { get; set; }
        public string branchAddress { get; set; }
        public string branchCode { get; set; }
        public bool isStaff { get; set; }
        public string customerCategory { get; set; }
        public string customerCategoryDesc { get; set; }
        public string accountOfficer { get; set; }
        public string strAddress1 { get; set; }
        public string strAddress2 { get; set; }
        public string strAddress3 { get; set; }
        public string strCity { get; set; }
        public string strState { get; set; }
        public string country { get; set; }
        public string compMIS2 { get; set; }
        public string compMIS4 { get; set; }
        public object compMIS8 { get; set; }
        public object tin { get; set; }
    }

}
