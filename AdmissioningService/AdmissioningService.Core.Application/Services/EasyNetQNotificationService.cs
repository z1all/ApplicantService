using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.Enums;
using Common.Models.Models;
using Common.ServiceBus.NotificationSender;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService;
using EasyNetQ;

namespace AdmissioningService.Core.Application.Services
{
    public class EasyNetQNotificationService : NotificationSender, INotificationService
    {
        public EasyNetQNotificationService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult> AddedManagerToApplicantAdmissionAsync(UserDTO manager, UserDTO applicant)
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

        public async Task<ExecutionResult> UpdatedAdmissionStatusAsync(AdmissionStatus newStatus, UserDTO applicant)
        {
            bool result = await SendingHandler(new AdmissionStatusUpdatedNotification()
            {
                Id = applicant.Id,
                Email = applicant.Email,
                FullName = applicant.FullName,
                NewStatus = newStatus,
            });

            return GiveResult(result, "An error occurred when sending a notification about admission status was updated.");
        }
    }
}
