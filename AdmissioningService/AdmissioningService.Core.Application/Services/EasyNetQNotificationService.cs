using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.Models;
using Common.ServiceBus.NotificationSender;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService;
using EasyNetQ;

namespace AdmissioningService.Core.Application.Services
{
    public class EasyNetQNotificationService : NotificationSender, INotificationService
    {
        public EasyNetQNotificationService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult> AddedManagerToApplicantAdmission(UserDTO manager, UserDTO applicant)
        {
            bool result = await SendingHandler(new ManagerAppointedNotification()
            {
                ManagerId = manager.Id,
                ManagerEmail = manager.Email,
                ManagerFullName = manager.FullName,

                ApplicantId = applicant.Id,
                ApplicantEmail = applicant.Email,
                ApplicantFullName = applicant.FullName,
            });

            return GiveResult(result, "An error occurred when sending a notification about manager was added to applicant admission.");
        }
    }
}
