using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class RpcAdmissionService : BaseRpcService, IAdmissionService
    {
        public RpcAdmissionService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult<ApplicantAdmissionPagedDTO>> GetApplicantAdmissionAsync(ApplicantAdmissionFilterDTO applicantAdmission, Guid managerId)
        {
            ExecutionResult<GetAdmissionsAsyncResponse> response
                = await RequestHandlerAsync<ExecutionResult<GetAdmissionsAsyncResponse>, GetAdmissionsAsyncRequest>(
                     new() { ApplicantAdmissionFilter = applicantAdmission, ManagerId = managerId }, "GetApplicantAdmissionFail");

            return ResponseHandler(response, manager => manager.ApplicantAdmissionPaged);
        }
    }
}
