using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class RpcApplicantService : BaseRpcService, IApplicantService
    {
        public RpcApplicantService(IBus bus) : base(bus){ }

        public async Task<ExecutionResult<ApplicantInfo>> GetApplicantInfoAsync(Guid applicantId)
        {
            ExecutionResult<GetApplicantInfoResponse> response
                = await RequestHandlerAsync<ExecutionResult<GetApplicantInfoResponse>, GetApplicantInfoRequest>(
                     new() { ApplicantId = applicantId }, "GetApplicantFail");

            return ResponseHandler(response, applicant => applicant.ApplicantInfo);
        }

        public async Task<ExecutionResult> ChangeAdditionInfoAsync(ChangeAdditionInfoDTO changeInfo, Guid managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangeApplicantInfoRequest>(
                new()
                {
                    ApplicantId = changeInfo.ApplicantId,
                    ManagerId = managerId,

                    Birthday = changeInfo.Birthday,
                    Citizenship = changeInfo.Citizenship,
                    Gender = changeInfo.Gender,
                    PhoneNumber = changeInfo.PhoneNumber
                }, "ChangeApplicantInfoFail");
        }
    }
}
