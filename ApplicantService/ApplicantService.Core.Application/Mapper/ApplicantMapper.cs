using ApplicantService.Core.Domain;
using Common.Models.DTOs.Applicant;

namespace ApplicantService.Core.Application.Mapper
{
    public static class ApplicantMapper
    {
        public static ApplicantInfo ToApplicantInfo(this Applicant applicant)
        {
            return new()
            {
                ApplicantProfile = applicant.ToApplicantProfile(),
                Documents = applicant.Documents.Select(document => new DocumentInfo()
                {
                    Id = document.Id,
                    Type = document.DocumentType,
                    Comments = document.Comments
                }).ToList(),
            };
        }

        public static ApplicantProfile ToApplicantProfile(this Applicant applicant)
        {
            return new ApplicantProfile()
            {
                Id = applicant.Id,
                Email = applicant.Email,
                FullName = applicant.FullName,
                Birthday = applicant.Birthday,
                Citizenship = applicant.Citizenship,
                Gender = applicant.Gender,
                PhoneNumber = applicant.PhoneNumber,
            };
        }
    }
}
