using MKopaMessageBox.Domain.DTO;
using MKopaMessageBox.Domain.DTOs;

namespace MKopaMessageBox.AppCore.Interfaces
{
    public interface IMessageRepository
    {
        Task<bool> SendSMS(SendSMSRequestDTO model);
        Task<string> Callback(CallbackDto model);
    }
}
