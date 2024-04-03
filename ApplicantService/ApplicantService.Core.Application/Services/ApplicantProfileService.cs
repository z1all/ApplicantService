using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Domain;
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
        public async Task<ExecutionResult<ApplicantProfile>> GetApplicantProfileAsync(Guid applicantId)
        {
            Applicant? applicant = await _profileRepository.GetByIdAsync(applicantId);
            if(applicant == null)
            {
                return new(keyError: "GetProfileFail", error: "Applicant not found! Try again later.");
            }

            return new()
            {
                Result = new ApplicantProfile()
                {
                    Email = applicant.User!.Email,
                    FullName = applicant.User!.Email,
                    Birthday = applicant.Birthday,
                    Citizenship = applicant.Citizenship,
                    Gender = applicant.Gender,
                    PhoneNumber = applicant.PhoneNumber,
                },
            }; 
        }

        public async Task<ExecutionResult> EditApplicantProfileAsync(EditApplicantProfile applicantProfile, Guid applicantId)
        {
            Applicant? applicant = await _profileRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                return new(keyError: "GetProfileFail", error: "Applicant not found! Try again later.");
            }

            applicant.Birthday = applicantProfile.Birthday;
            applicant.Citizenship = applicantProfile.Citizenship;
            applicant.Gender = applicantProfile.Gender;
            applicant.PhoneNumber = applicantProfile.PhoneNumber;

            await _profileRepository.UpdateAsync(applicant);

            return new(isSuccess: true);
        }
    }
}
