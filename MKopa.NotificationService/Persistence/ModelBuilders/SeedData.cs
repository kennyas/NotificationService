using MKopaMessageBox.Domain.Entities;

namespace MKopaMessageBox.Persistence.ModelBuilders
{
    public static class AccessKeyData
    {
        public static List<AccessKey> GetAccessKeys()
        {
            return new List<AccessKey>
                {
                    new AccessKey
                    {ChannelName ="MKopa",KeyName = "XApiKey",KeyValue = Guid.NewGuid().ToString() }
                };
        }
    }
}
