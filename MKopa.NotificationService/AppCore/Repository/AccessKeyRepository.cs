using Microsoft.Extensions.Options;
using MKopaMessageBox.AppCore.Interfaces;
using MKopaMessageBox.Domain.DTOs;
using MKopaMessageBox.Domain.Entities;

namespace MKopaMessageBox.AppCore.Repository
{
    public class AccessKeyRepository : IAccessKeyRepository
    {
        private readonly ILogger<AccessKeyRepository> _logger;
        private readonly AppSettings _appSettings;

        public AccessKeyRepository( ILogger<AccessKeyRepository> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

       
    }
}
