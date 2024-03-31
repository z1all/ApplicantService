using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models;

namespace ApplicantService.Core.Application.Services
{
    public class ApplicantProfileService : IApplicantProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ApplicantProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public Task<ExecutionResult> EditApplicantProfileAsync(EditApplicantProfile applicantProfile, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult<ApplicantProfile>> GetApplicantProfileAsync(Guid applicantId)
        {
            throw new NotImplementedException();
        }
    }
}
