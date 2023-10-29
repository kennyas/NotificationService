using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text;
using MKopaMessageBox.AppCore.Interfaces;
using MKopaMessageBox.Domain.DTOs;
using RestSharp;
using MKopaMessageBox.Domain.DTO;
using MKopa.NotificationService.Utils;

namespace MKopaMessageBox.AppCore.Repository
{

    public class MessageRepository : IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;
        private readonly IRestClientService _restClientService;
        private readonly AppSettings _appSettings;
        private readonly SmsConfig _smsConfig;
        public MessageRepository(ILogger<MessageRepository> logger
            , IOptions<AppSettings> appSettings
            , IRestClientService restClientService
            , IOptions<SmsConfig> smsConfig)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _restClientService = restClientService;
            _smsConfig = smsConfig.Value;
        }


        public async Task<bool> SendSMS(SendSMSRequestDTO model)
        {
            string msgResponse = string.Empty;
            try
            {
                var phone = string.Empty;
                if (string.IsNullOrWhiteSpace(model.MSIDN))
                {
                    _logger.LogError("could not get the MSIDN!");
                    return false;
                }

                var tokenRes = await GetAccessToken(); //await GetToken();
                var token = tokenRes.data.token;
                _logger.LogInformation($"Token {token}");

                var payload = new MessageRequestDTO
                {
                    dlr_url = _smsConfig.CallbackUrl,
                    message = model.SMSBody,
                    sender = "MKopa",
                    message_id = Guid.NewGuid().ToString(),
                    message_type = "Transactional",
                    msisdns = new List<string> { model.MSIDN }
                };

                var messageResponse = await _restClientService.PostAsync<SmsResponseDto<MessageDto>, MessageRequestDTO>($"{_smsConfig.Url}messaging/send", payload, new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } });
                _logger.LogInformation($"[Send Sms]: Url {_smsConfig.Url}, Response => {messageResponse}");
                //test for valid response
                if (messageResponse.status == 200)
                {
                    _logger.LogInformation("[MessageServiceRepository][Send Sms] Send Sms Success: Sent Sms successfully");
                    return true;

                }
                else
                {
                    _logger.LogInformation($"[MessageServiceRepository][Send Sms] Send Sms Failed: failed to send due to {messageResponse.message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return false;
            }
        }

        public async Task<string> Callback(CallbackDto model)
        {
            try
            {
                bool status = false;
                _logger.LogInformation($"[MessageServiceRepository][Send Sms] Send Sms Failed: failed to send due to {model.status}");
                switch (model.status)
                {
                    case "DeliveredToTerminal":
                    case "DELIVRD":
                    case "SubmittedToNetwork":
                        status = true; break;
                    case "billingFailed":
                    case "invalidNumber":
                    case "NACK%2F0x0000000a%2FInvalid%20Source%20Address":
                    case "SenderName%20Blacklisted":
                    case "DeliveryImpossible":
                    case "UNDELIV":
                    case "AbsentSubscriber":
                        status = false; break;
                }

                return "Callback successfully recorded";
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return "Something went wrong";
            }
        }

        private async Task<SmsResponseDto<AuthDto>> GetAccessToken()
        {
            try
            {
                var authPayload = new SmsAuthRequestDto
                {
                    password = _smsConfig.Password,
                    username = _smsConfig.Username,
                };

                var auth = await _restClientService.PostAsync<SmsResponseDto<AuthDto>, SmsAuthRequestDto>($"{_smsConfig.Url}auth/token", authPayload, new Dictionary<string, string>());

                return auth;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new SmsResponseDto<AuthDto>();
            }
        }
        
    }

}
