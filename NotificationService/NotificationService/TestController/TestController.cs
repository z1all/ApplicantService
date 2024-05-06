using Microsoft.AspNetCore.Mvc;
using NotificationService.Services.Interfaces;

namespace NotificationService.TestController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(ISmtpService smtpService) : ControllerBase
    {
        [HttpPost]
        public async Task SendEmail(string message)
        {
            await smtpService.SendAsync("Test", "mazhur04@gmail.com", "", message);
        }
    }
}
