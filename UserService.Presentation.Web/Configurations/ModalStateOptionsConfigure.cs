using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace UserService.Presentation.Web.Options
{
    public class ModalStateOptionsConfigure : IConfigureOptions<ApiBehaviorOptions>
    {
        public void Configure(ApiBehaviorOptions options)
        {
            // Отключение автоматической проверки ModelState.IsValid
            options.SuppressModelStateInvalidFilter = true; 
        }
    }
}
