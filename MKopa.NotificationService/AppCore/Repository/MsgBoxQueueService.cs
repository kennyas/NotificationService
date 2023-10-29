using Microsoft.Extensions.Options;
using MKopa.NotificationService.AppCore.Interfaces;
using MKopa.NotificationService.AppCore.Repository;
using MKopa.NotificationService.Domain.DTO;
using MKopa.NotificationService.Enums;
using MKopaMessageBox.AppCore.Interfaces;
using MKopaMessageBox.Domain.DTO;
using MKopaMessageBox.Domain.DTOs;
using System.Text.Json;

namespace MKopaMessageBox.AppCore.Repository
{
    public class MsgBoxQueueService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSettings _appsettings;
        private readonly ILogger<MsgBoxQueueService> _logger;

        public MsgBoxQueueService(ILogger<MsgBoxQueueService> logger, IServiceProvider serviceProvider, IOptions<AppSettings> appsettings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _appsettings = appsettings.Value;
        }


        public async Task<GenResponse<bool>> RabbitProcessMessageSMS(CancellationToken ct = default)
        {
            GenResponse<bool> objResp = new();
            FetchQueueProps MqSMSOptions = new()
            {
                QueueName = QueueNameOrRouteKeyEnums.SmsMessages.ToString(),
                RoutingKey = QueueNameOrRouteKeyEnums.GeneralSMSKey.ToString(),
                IsAutoAcknowledged = _appsettings.MsgQueue.IsAutoAcknowledged
            };
            try
            {
                var scope = _serviceProvider.CreateScope();
                var msgQueueSvc = scope.ServiceProvider.GetRequiredService<IMessageQueueService>();
                var messageRepoSvc = scope.ServiceProvider.GetRequiredService<IMessageRepository>();


                GenResponse<string> smsObj = await msgQueueSvc.FetchAndProcessMsgFromQueue(MqSMSOptions, ct: ct);
                if (smsObj != null && smsObj.IsSuccess)
                {
                    SendSMSRequestDTO smsModel = JsonSerializer.Deserialize<SendSMSRequestDTO>(smsObj.Result);
                    if (smsModel != null)
                    {
                        var objResult = await messageRepoSvc.SendSMS(smsModel);
                        if (!objResult)
                        {
                            MqPublisherProps MqSendEmailOptions = new()
                            {
                                ClientProvidedName = ClientProvidedNameEnum.MkopaLoanSvc,
                                ExchangeName = ExchangeNameEnum.MKopaLoanSvc_Exchange,
                                QueueName = QueueNameOrRouteKeyEnums.SmsMessages.ToString(),
                                RoutingKey = $"{QueueNameOrRouteKeyEnums.GeneralSMSKey.ToString()}"
                            };
                            _ = await msgQueueSvc.RabbitMqPublish<SendSMSRequestDTO>(smsModel, MqSendEmailOptions);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GenResponse<bool>.Failed(ex.Message);
            }
            return objResp;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                System.Diagnostics.Stopwatch stopwatch = new();
                stopwatch.Start();

                Thread.Sleep(_appsettings.MsgQueue.DelayInMilliseconds);

                await this.RabbitProcessMessageSMS(stoppingToken);

                stopwatch.Stop();
                Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
