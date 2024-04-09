using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;

namespace DictionaryService.Infrastructure.Persistence
{
    internal static class AppDbSeed
    {
        public static void AddUpdateStatuses(AppDbContext dbContext)
        {
            UpdateStatus[] updateStatuses =
            [
                new UpdateStatus()
                {
                    DictionaryType = Core.Domain.Enum.DictionaryType.Faculty,
                    Status = Core.Domain.Enum.UpdateStatusEnum.Updated,
                },
                new UpdateStatus()
                {
                    DictionaryType = Core.Domain.Enum.DictionaryType.EducationProgram,
                    Status = Core.Domain.Enum.UpdateStatusEnum.Updated,
                },
                new UpdateStatus()
                {
                    DictionaryType = Core.Domain.Enum.DictionaryType.EducationDocumentType,
                    Status = Core.Domain.Enum.UpdateStatusEnum.Updated,
                },
                new UpdateStatus()
                {
                    DictionaryType = Core.Domain.Enum.DictionaryType.EducationLevel,
                    Status = Core.Domain.Enum.UpdateStatusEnum.Updated,
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
