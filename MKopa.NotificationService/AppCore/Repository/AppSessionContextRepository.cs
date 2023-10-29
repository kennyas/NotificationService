using Microsoft.Extensions.Primitives;
using OnaxTools.Dto.Identity;
using PrimusCommon.DTO;
using MKopaMessageBox.AppCore.Interfaces;
using MKopaMessageBox.Domain.Entities;
using MKopaMessageBox.MKopaMessageBox.Domain.DTOs;
using System.Text;

namespace MKopaMessageBox.AppCore.Repository
{
    public class AppSessionContextRepository: IAppSessionContextRepository
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAccessKeyRepository _accessKeyRepository;

        public AppSessionContextRepository(IHttpContextAccessor contextAccessor, IAccessKeyRepository accessKeyRepository)
        {
            _contextAccessor = contextAccessor;
            _accessKeyRepository = accessKeyRepository;
        }

        public async Task<AppSessionData<AccessKey>> GetUserDataFromSession()
        {
            AppSessionData<AccessKey> objResp = new();
            try
            {
                StringValues callerKeyValue;
                _contextAccessor.HttpContext.Request.Headers.TryGetValue(AppConstants.DefaultHeaderKeyName, out callerKeyValue);
                if (String.IsNullOrWhiteSpace(callerKeyValue))
                {
                    return objResp;
                }
                else
                {
                    _contextAccessor.HttpContext.Session.TryGetValue(key: callerKeyValue, out byte[] userDataFromSession);
                    if (userDataFromSession != null && userDataFromSession.Any())
                    {
                        objResp.Data = await _accessKeyRepository.GetActiveAccessKeyByKeyValue(callerKeyValue);                        
                    }
                }
            }
            catch (Exception ex)
            {
                OnaxTools.Logger.LogException(ex);
            }
            return objResp;
        }
    }
}
