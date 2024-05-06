using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToUserDTO(this UserCache user)
        {
            return new()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
            };
        }

        public static UserDTO ToUserDTO(this ApplicantCache applicant)
        {
            return new()
            {
                Id = applicant.Id,
                Email = applicant.Email,
                FullName = applicant.FullName,
            };
        }
    }
}
