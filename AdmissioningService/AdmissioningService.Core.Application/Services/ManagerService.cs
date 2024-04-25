using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
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
        private readonly IRequestService _requestService;

        public ManagerService(
            IUserCacheRepository userCacheRepository,
            IManagerRepository managerRepository,
            IFacultyCacheRepository facultyCacheRepository,
            IRequestService requestService)
        {
            _userCacheRepository = userCacheRepository;
            _managerRepository = managerRepository;
            _facultyCacheRepository = facultyCacheRepository;
            _requestService = requestService;
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
