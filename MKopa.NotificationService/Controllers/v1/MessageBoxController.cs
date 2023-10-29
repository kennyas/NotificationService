using Microsoft.AspNetCore.Mvc;
using MKopaMessageBox.AppCore.Interfaces;
using MKopaMessageBox.Domain.DTO;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MKopaMessageBox.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessageBoxController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        public MessageBoxController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpPost(nameof(SendSms))]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "θΞ> - Send SMS.")]
        public async Task<IActionResult> SendSms(SendSMSRequestDTO model)
        {
            var objResp = await _messageRepository.SendSMS(model);
            return Ok(objResp);
        }

        [HttpGet(nameof(SmsCallback))]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "θD> - Sms Callback endpoint.")]
        public async Task<IActionResult> SmsCallback(CallbackDto model)
        {
            var objResp = await _messageRepository.Callback(model);
            return Ok(objResp);
        }
    }
}
