using Microsoft.Extensions.DependencyInjection;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Services;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators;

namespace DictionaryService.Core.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUpdateDictionaryService, UpdateDictionaryService>();
            services.AddScoped<IDictionaryInfoService, DictionaryInfoService>();
            services.AddScoped<INotificationService, EasyNetQNotificationService>();

            // Action creators
            services.AddScoped<UpdateActionsCreator<Faculty, FacultyExternalDTO>, UpdateFacultyActionsCreator>();
            services.AddScoped<UpdateActionsCreator<EducationLevel, EducationLevelExternalDTO>, UpdateEducationLevelActionsCreator>();
            services.AddScoped<UpdateActionsCreator<EducationProgram, EducationProgramExternalDTO>, UpdateEducationProgramActionsCreator>();
            services.AddScoped<UpdateActionsCreator<EducationDocumentType, EducationDocumentTypeExternalDTO>, UpdateEducationDocumentTypeActionsCreator>();
            
            return services;
        }
    }
}
