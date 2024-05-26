using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs.Admission;
using Common.Models.Enums;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class RpcAdmissionService : BaseRpcService, IAdmissionService
    {
        public RpcAdmissionService(ILogger<RpcAdmissionService> logger, IBus bus) 
            : base(logger, bus) { }

        public async Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, CheckPermissionsRequest>(new()
            {
                ApplicantId = applicantId,
                ManagerId = managerId
            }, "CheckPermissionsFail");
        }

        public async Task<ExecutionResult<ApplicantAdmissionPagedDTO>> GetAdmissionsAsync(ApplicantAdmissionFilterDTO applicantAdmission, Guid managerId)
        {
            ExecutionResult<GetAdmissionsResponse> response
                = await RequestHandlerAsync<ExecutionResult<GetAdmissionsResponse>, GetAdmissionsRequest>(
                     new() { ApplicantAdmissionFilter = applicantAdmission, ManagerId = managerId }, "GetAdmissionsFail");

            return ResponseHandler(response, manager => manager.ApplicantAdmissionPaged);
        }

        public async Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId)
        {
            ExecutionResult<GetApplicantAdmissionResponse> response
               = await RequestHandlerAsync<ExecutionResult<GetApplicantAdmissionResponse>, GetApplicantAdmissionRequest>(
                    new() { ApplicantId = applicantId, AdmissionId = admissionId }, "GetApplicantAdmissionFail");

            return ResponseHandler(response, manager => manager.ApplicantAdmission);
        }

        public async Task<ExecutionResult> AddManagerToAdmissionAsync(Guid admissionId, Guid? managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, AddManagerToAdmissionRequest>(
                    new() { AdmissionId = admissionId, MangerId = managerId }, "AddManagerToAdmissionFail");
        }

        public async Task<ExecutionResult> TakeApplicantAdmissionAsync(Guid admissionId, Guid managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, TakeApplicantAdmissionRequest>(
                    new() { AdmissionId = admissionId, MangerId = managerId }, "TakeApplicantAdmissionFail");
        }

        public async Task<ExecutionResult> RejectApplicantAdmissionAsync(Guid admissionId, Guid managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, RejectApplicantAdmissionRequest>(
                   new() { AdmissionId = admissionId, MangerId = managerId }, "RejectApplicantAdmissionFail");
        }

        public async Task<ExecutionResult> ChangeAdmissionStatusAsync(Guid admissionId, Guid managerId, ManagerChangeAdmissionStatus changeStatus)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangeAdmissionStatusRequest>(
                new() { AdmissionId = admissionId, NewStatus = changeStatus, ManagerId = managerId }, "ChangeAdmissionStatusFail");
        }

        public async Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionsCompaniesAsync()
        {
            ExecutionResult<GetAdmissionCompaniesResponse> response
                = await RequestHandlerAsync<ExecutionResult<GetAdmissionCompaniesResponse>, GetAdmissionCompaniesRequest>(
                     new(), "GetAdmissionsCompaniesFail");

            return ResponseHandler(response, companies => companies.AdmissionCompanies);
        }

        public async Task<ExecutionResult> CreateAdmissionCompanyAsync(int year)
        {
            return await RequestHandlerAsync<ExecutionResult, CreateAdmissionCompanyRequest>(
                new() { Year = year }, "CreateAdmissionCompanyFail");
        }
    }
}
