using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Models.Enums;

namespace DictionaryService.Infrastructure.Persistence
{
    internal static class AppDbSeed
    {
        public static void AddUpdateStatuses(UpdateStatusDbContext dbContext)
        {
            UpdateStatus[] updateStatuses =
            [
                new UpdateStatus()
                {
                    DictionaryType = DictionaryType.Faculty,
                    Status = UpdateStatusEnum.Updated,
                },
                new UpdateStatus()
                {
                    DictionaryType = DictionaryType.EducationProgram,
                    Status = UpdateStatusEnum.Updated,
                },
                new UpdateStatus()
                {
                    DictionaryType = DictionaryType.EducationDocumentType,
                    Status = UpdateStatusEnum.Updated,
                },
                new UpdateStatus()
                {
                    DictionaryType = DictionaryType.EducationLevel,
                    Status = UpdateStatusEnum.Updated,
                },
            ];

            List<UpdateStatus> existUpdateStatuses = dbContext.UpdateStatuses.ToList();
            foreach (var updateStatus in updateStatuses)
            {
                if (!existUpdateStatuses.Any(existUpdateStatus => existUpdateStatus.DictionaryType == updateStatus.DictionaryType))
                {
                    dbContext.Add(updateStatus);
                }
            }
            dbContext.SaveChanges();
        }
    }
}
