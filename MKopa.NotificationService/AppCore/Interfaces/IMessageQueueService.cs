using MKopa.NotificationService.AppCore.Repository;
using MKopa.NotificationService.Domain.DTO;
using System.Runtime.CompilerServices;

namespace MKopa.NotificationService.AppCore.Interfaces
{
    public interface IMessageQueueService
    {
        Task<GenResponse<string>> FetchAndProcessMsgFromQueue(FetchQueueProps props, CancellationToken ct = default, [CallerMemberName] string caller = "");
        Task<GenResponse<bool>> RabbitMqPublish<T>(T data, MqPublisherProps props, CancellationToken ct = default, [CallerMemberName] string caller = "");
    }
}
