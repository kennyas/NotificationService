using MKopaMessageBox.Domain.Entities;

namespace MKopaMessageBox.AppCore.Interfaces
{
    public interface IAppSessionContextRepository
    {
        Task<AppSessionData<AccessKey>> GetUserDataFromSession();
    }
}
