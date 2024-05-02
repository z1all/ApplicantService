﻿using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;

namespace AdmissioningService.Core.Application.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IUserCacheRepository _userCacheRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IFacultyCacheRepository _facultyCacheRepository;
        private readonly IApplicantAdmissionRepository _applicantAdmissionRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;
        private readonly IRequestService _requestService;
        private readonly INotificationService _notificationService;

        public ManagerService(
            IUserCacheRepository userCacheRepository, INotificationService notificationService,
            IFacultyCacheRepository facultyCacheRepository, IManagerRepository managerRepository,
            IApplicantAdmissionStateMachin applicantAdmissionStateMachin, IRequestService requestService,
            IApplicantAdmissionRepository applicantAdmissionRepository)
        {
            _userCacheRepository = userCacheRepository;
            _managerRepository = managerRepository;
            _facultyCacheRepository = facultyCacheRepository;
            _applicantAdmissionRepository = applicantAdmissionRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;
            _requestService = requestService;
            _notificationService = notificationService;
        }

        public async Task<ExecutionResult> CreateManagerAsync(CreateManagerDTO createManager)
        {
            if (createManager.FacultyId is not null)
            {
                ExecutionResult result = await CheckFacultyAsync((Guid)createManager.FacultyId);
                if (!result.IsSuccess) return result;
            }

            UserCache newUser = new()
            {
                Id = createManager.Id,
                FullName = createManager.FullName,
                Email = createManager.Email,
            };

            Manager newManager = new()
            {
                Id = createManager.Id,
                FacultyId = createManager.FacultyId,
                User = newUser,
            };
            await _managerRepository.AddAsync(newManager);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> DeleteManagerAsync(Guid managerId)
        {
            UserCache? user = await _userCacheRepository.GetByIdAsync(managerId);
            if (user is null)
            {
                return new(keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            await _userCacheRepository.DeleteAsync(user);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> TakeApplicantAdmissionAsync(Guid admissionId, Guid managerId)
        {
            Manager? manager = await _managerRepository.GetByIdWithUserAsync(managerId);
            if (manager is null)
            {
                return new(keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdWithApplicantAsync(admissionId);
            if (applicantAdmission is null)
            {
                return new(keyError: "ApplicantAdmissionFound", error: $"Applicant admission with id {managerId} not found!");
            }

            if (applicantAdmission.ManagerId is not null && applicantAdmission.ManagerId != managerId)
            {
                return new(keyError: "ManagerAlreadyExist", error: "This applicant admission was taken by other manager!");
            }
            else if (applicantAdmission.ManagerId == managerId)
            {
                return new(keyError: "ApplicantAdmissionAlreadyTaken", error: "This applicant admission was taken by this manager!");
            }

            await _applicantAdmissionStateMachin.AddManagerAsync(applicantAdmission, manager);

            return await _notificationService
                .AddedManagerToApplicantAdmission(manager.User!.ToUserDTO(), applicantAdmission.Applicant!.ToUserDTO());
        }

        public async Task<ExecutionResult> RefuseFromApplicantAdmissionAsync(Guid admissionId, Guid managerId)
        {
            Manager? manager = await _managerRepository.GetByIdAsync(managerId);
            if (manager is null)
            {
                return new(keyError: "ManagerNotFound", error: $"Manager with id {managerId} not found!");
            }

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdAsync(admissionId);
            if (applicantAdmission is null)
            {
                return new(keyError: "ApplicantAdmissionFound", error: $"Applicant admission with id {managerId} not found!");
            }

            if (applicantAdmission.ManagerId != managerId)
            {
                return new(keyError: "ApplicantNotAppertain", error: $"Admission with id {admissionId} doesn't appertain to this manager!");
            }

            await _applicantAdmissionStateMachin.DeleteManagerAsync(applicantAdmission);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<List<ManagerDTO>>> GetManagersAsync()
        {
            List<Manager> managers = await _managerRepository.GetAllWithFacultyAndUserAsync();

            return new()
            {
                Result = managers.Select(manager => manager.ToManagerDTO()).ToList(),
            };
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
    }
}
