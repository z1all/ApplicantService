using Microsoft.Extensions.Logging;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Domain;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using Common.Models.Models;
using Common.Models.Enums;
using Common.Models.DTOs.Admission;
using Common.Models.DTOs.Dictionary;
using AdmissioningService.Core.Application.Helpers;

namespace AdmissioningService.Core.Application.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ILogger<ManagerService> _logger;
        private readonly IUserCacheRepository _userCacheRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IFacultyCacheRepository _facultyCacheRepository;
        private readonly IApplicantAdmissionRepository _applicantAdmissionRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;
        private readonly IRequestService _requestService;
        private readonly INotificationService _notificationService;
        private readonly AdmissionHelper _admissionHelper;

        public ManagerService(
            ILogger<ManagerService> logger,
            IUserCacheRepository userCacheRepository, INotificationService notificationService,
            IFacultyCacheRepository facultyCacheRepository, IManagerRepository managerRepository,
            IApplicantAdmissionStateMachin applicantAdmissionStateMachin, IRequestService requestService,
            IApplicantAdmissionRepository applicantAdmissionRepository, AdmissionHelper admissionHelper)
        {
            _logger = logger;
            _userCacheRepository = userCacheRepository;
            _managerRepository = managerRepository;
            _facultyCacheRepository = facultyCacheRepository;
            _applicantAdmissionRepository = applicantAdmissionRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;
            _requestService = requestService;
            _notificationService = notificationService;
            _admissionHelper = admissionHelper;
        }

        public async Task<ExecutionResult> CreateManagerAsync(Guid managerId, ManagerDTO createManager)
        {
            if (createManager.FacultyId is not null)
            {
                ExecutionResult result = await CheckFacultyAsync((Guid)createManager.FacultyId);
                if (!result.IsSuccess) return result;
            }

            UserCache newUser = new()
            {
                Id = managerId,
                FullName = createManager.FullName,
                Email = createManager.Email,
            };

            Manager newManager = new()
            {
                Id = managerId,
                FacultyId = createManager.FacultyId,
                User = newUser,
            };
            await _managerRepository.AddAsync(newManager);

            _logger.LogInformation($"Created manager with id {managerId}");

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> ChangeManagerAsync(Guid managerId, ManagerDTO changeManager)
        {
            if (changeManager.FacultyId is not null)
            {
                ExecutionResult result = await CheckFacultyAsync((Guid)changeManager.FacultyId);
                if (!result.IsSuccess) return result;
            }

            Manager? manager = await _managerRepository.GetByIdWithUserAsync(managerId);
            if (manager is null)
            {
                _logger.LogInformation($"Manager with id {managerId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            manager.FacultyId = changeManager.FacultyId;
            manager.User!.FullName = changeManager.FullName;
            manager.User.Email = changeManager.Email;

            await _managerRepository.UpdateAsync(manager);

            _logger.LogInformation($"Updated manager with id {managerId}");

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> DeleteManagerAsync(Guid managerId)
        {
            UserCache? user = await _userCacheRepository.GetByIdAsync(managerId);
            if (user is null)
            {
                _logger.LogInformation($"Manager with id {managerId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            List<ApplicantAdmission> managerAdmissions = await _applicantAdmissionRepository.GetAllByManagerIdAsync(managerId);
            await _applicantAdmissionStateMachin.DeleteManagerRangeAsync(managerAdmissions);

            await _userCacheRepository.DeleteAsync(user);

            _logger.LogInformation($"Deleted manager with id {managerId}");

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> AddManagerToAdmissionAsync(Guid admissionId, Guid? managerId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdWithApplicantAsync(admissionId);
            if (applicantAdmission is null)
            {
                _logger.LogInformation($"Applicant admission with id {admissionId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ApplicantAdmissionFound", error: $"Applicant admission with id {admissionId} not found!");
            }

            if (managerId is null)
            {
                await _applicantAdmissionStateMachin.DeleteManagerAsync(applicantAdmission);
                return new(isSuccess: true);
            }
            else
            {
                Manager? manager = await _managerRepository.GetByIdWithUserAsync((Guid)managerId);
                if (manager is null)
                {
                    _logger.LogInformation($"Manager with id {managerId} not found!");
                    return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
                }

                await _applicantAdmissionStateMachin.AddManagerAsync(applicantAdmission, manager);

                _logger.LogInformation($"Manager with id {managerId} added to admission with id {admissionId}");

                return await _notificationService
                    .AddedManagerToApplicantAdmissionAsync(manager.User!.ToUserDTO(), applicantAdmission.Applicant!.ToUserDTO());
            }
        }

        public async Task<ExecutionResult> TakeApplicantAdmissionAsync(Guid admissionId, Guid managerId)
        {
            Manager? manager = await _managerRepository.GetByIdWithUserAsync(managerId);
            if (manager is null)
            {
                _logger.LogInformation($"Manager with id {managerId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdWithApplicantAsync(admissionId);
            if (applicantAdmission is null)
            {
                _logger.LogInformation($"Applicant admission with id {admissionId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ApplicantAdmissionFound", error: $"Applicant admission with id {admissionId} not found!");
            }

            if (applicantAdmission.ManagerId is not null && applicantAdmission.ManagerId != managerId)
            {
                _logger.LogInformation($"Applicant admission with id {admissionId} was taken by other manager");
                return new(StatusCodeExecutionResult.BadRequest, keyError: "ManagerAlreadyExist", error: "This applicant admission was taken by other manager!");
            }
            else if (applicantAdmission.ManagerId == managerId)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "ApplicantAdmissionAlreadyTaken", error: "This applicant admission was taken by this manager!");
            }

            await _applicantAdmissionStateMachin.AddManagerAsync(applicantAdmission, manager);

            _logger.LogInformation($"Manager with id {managerId} added to admission with id {admissionId}");

            return await _notificationService
                .AddedManagerToApplicantAdmissionAsync(manager.User!.ToUserDTO(), applicantAdmission.Applicant!.ToUserDTO());
        }

        public async Task<ExecutionResult> RefuseFromApplicantAdmissionAsync(Guid admissionId, Guid managerId)
        {
            Manager? manager = await _managerRepository.GetByIdAsync(managerId);
            if (manager is null)
            {
                _logger.LogInformation($"Manager with id {managerId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdAsync(admissionId);
            if (applicantAdmission is null)
            {
                _logger.LogInformation($"Applicant admission with id {admissionId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ApplicantAdmissionFound", error: $"Applicant admission with id {managerId} not found!");
            }

            if (applicantAdmission.ManagerId != managerId)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "ApplicantAdmissionNotAppertain", error: $"Admission with id {admissionId} doesn't appertain to this manager!");
            }

            await _applicantAdmissionStateMachin.DeleteManagerAsync(applicantAdmission);

            _logger.LogInformation($"Manager with id {managerId} deleted from admission with id {admissionId}");

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<List<ManagerProfileDTO>>> GetManagersAsync()
        {
            List<Manager> managers = await _managerRepository.GetAllWithFacultyAndUserAsync();

            return new()
            {
                Result = managers.Select(manager => manager.ToManagerDTO()).ToList(),
            };
        }

        public async Task<ExecutionResult<ManagerProfileDTO>> GetManagerAsync(Guid managerId)
        {
            Manager? manager = await _managerRepository.GetByIdWithFacultyAndUserAsync(managerId);
            if (manager is null)
            {
                _logger.LogInformation($"Manager with id {managerId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            return new() { Result = manager.ToManagerDTO() };
        }

        private async Task<ExecutionResult> CheckFacultyAsync(Guid facultyId)
        {
            bool existFaculty = await _facultyCacheRepository.AnyByIdAsync(facultyId);
            if (!existFaculty)
            {
                ExecutionResult<GetFacultyResponse> result = await _requestService.GetFacultyAsync(facultyId);
                if (!result.IsSuccess) return result;
                FacultyDTO faculty = result.Result!.Faculty;

                FacultyCache newFaculty = faculty.ToFacultyCache();
                await _facultyCacheRepository.AddAsync(newFaculty);
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> ChangeApplicantAdmissionStatusAsync(Guid admissionId, ManagerChangeAdmissionStatus changeAdmissionStatus, Guid managerId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdAsync(admissionId);
            if (applicantAdmission is null)
            {
                _logger.LogInformation($"Applicant admission with id {admissionId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "AdmissionNotFound", error: $"Admission with id {admissionId} not found!");
            }

            Manager? manager = await _managerRepository.GetByIdAsync(managerId);
            if (manager is null)
            {
                _logger.LogInformation($"Manager with id {managerId} not found!");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            ExecutionResult canEdit = await _admissionHelper.CheckPermissionsAsync(applicantAdmission.ApplicantId, managerId);
            if (!canEdit.IsSuccess) return canEdit;

            //if (manager.FacultyId is not null && managerId != applicantAdmission.ManagerId)
            //{
            //    return new(StatusCodeExecutionResult.BadRequest, keyError: "ApplicantAdmissionNotAppertain", error: $"Admission with id {admissionId} doesn't appertain to this manager!");
            //}

            await _applicantAdmissionStateMachin.ChangeAdmissionStatusAsync(applicantAdmission, changeAdmissionStatus);

            return new(isSuccess: true);
        }
    }
}
