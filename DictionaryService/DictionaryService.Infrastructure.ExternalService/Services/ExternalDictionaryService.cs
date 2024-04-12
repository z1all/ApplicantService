using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;
using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Infrastructure.ExternalService.Configurations;
using Common.Models;
using Microsoft.AspNetCore.Routing;

namespace DictionaryService.Infrastructure.ExternalService.Services
{
    public class ExternalDictionaryService : IExternalDictionaryService
    {
        private readonly WebExternalOptions _externalOptions;
        private readonly HttpClient _httpClient;

        public ExternalDictionaryService(HttpClient httpClient, IOptions<WebExternalOptions> externalOptions)
        {
            _externalOptions = externalOptions.Value;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_externalOptions.BaseUrl);
            
            _httpClient.DefaultRequestHeaders
                .Add(HeaderNames.Accept, "text/plain");
            _httpClient.DefaultRequestHeaders
                .Add(HeaderNames.Authorization, _externalOptions.AuthorizationHeader);
        }

        public async Task<ExecutionResult<List<EducationDocumentTypeExternalDTO>>> GetEducationDocumentTypesAsync() =>
             await GetAsync<List<EducationDocumentTypeExternalDTO>>(_externalOptions.DocumentTypeRoute, "GetEducationDocumentsFail");

        public async Task<ExecutionResult<List<EducationLevelExternalDTO>>> GetEducationLevelsAsync() =>
             await GetAsync<List<EducationLevelExternalDTO>>(_externalOptions.EducationLevelRoute, "GetEducationLevelsFail");

        public async Task<ExecutionResult<List<FacultyExternalDTO>>> GetFacultiesAsync() => 
             await GetAsync<List<FacultyExternalDTO>> (_externalOptions.FacultiesRoute, "GetFacultiesFail");

        public async Task<ExecutionResult<List<EducationProgramExternalDTO>>> GetEducationProgramAsync()
        {
            ProgramListExternalDTO? programListExternal
                = await _httpClient.GetFromJsonAsync<ProgramListExternalDTO>(_externalOptions.EducationProgramRoute);
            if (programListExternal is null)
            {
                return new("GetEducationProgramFail", error: "Unknown error");
            }

            return new()
            {
                Result = programListExternal.Programs,
            };
        }

        private async Task<ExecutionResult<TResult>> GetAsync<TResult>(string route, string keyError)
        {
            TResult? result
                = await _httpClient.GetFromJsonAsync<TResult>(route);
            if (result is null)
            {
                return new(keyError, error: "Unknown error");
            }

            return new()
            {
                Result = result,
            };
        }
    }
}
