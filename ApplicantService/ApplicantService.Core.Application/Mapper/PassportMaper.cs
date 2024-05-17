using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Domain;
using Common.Models.Enums;

namespace ApplicantService.Core.Application.Mappers
{
    public static class PassportMapper
    {
        public static PassportInfo ToPassportInfo(this Passport passport)
        {
            return new()
            {
                Id = passport.Id,
                BirthPlace = passport.BirthPlace,
                IssuedByWhom = passport.IssuedByWhom,
                IssueYear = passport.IssueYear,
                SeriesNumber = passport.SeriesNumber,
                Scans = passport.FilesInfo.Select(file => new ScanInfo()
                {
                    Id = file.Id,
                    Name = file.Name,
                    Type = file.Type
                }).ToList()
            };
        }

        public static Passport ToPassport(this EditAddPassportInfo passportInfo, Guid applicantId)
        {
            return new()
            {
                ApplicantId = applicantId,
                ApplicantIdCache = applicantId,
                DocumentType = DocumentType.Passport,
                BirthPlace = passportInfo.BirthPlace,
                IssuedByWhom = passportInfo.IssuedByWhom,
                IssueYear = passportInfo.IssueYear,
                SeriesNumber = passportInfo.SeriesNumber,
            };
        }
    }
}
