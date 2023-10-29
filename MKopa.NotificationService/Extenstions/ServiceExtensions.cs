using Microsoft.Extensions.Options;
using MKopaMessageBox.AppCore.Interfaces;
using MKopaMessageBox.AppCore.Repository;
using MKopaMessageBox.Domain.DTO;
using MKopaMessageBox.Domain.DTOs;
using Serilog;
using MKopa.NotificationService.Utils;
using MKopa.NotificationService.AppCore.Interfaces;
using MKopa.NotificationService.AppCore.Repository;

namespace MKopaMessageBox.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServicesAndDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            services.Configure<SmsConfig>(configuration.GetSection(nameof(SmsConfig))); 
            

            services.AddScoped<IMessageQueueService, MessageQueueService>();
            services.AddHttpContextAccessor();

            services.AddHostedService<MsgBoxQueueService>();

            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRestClientService, RestClientService>(); 
            services.AddScoped<IAccessKeyRepository, AccessKeyRepository>();
        }
    }
}
