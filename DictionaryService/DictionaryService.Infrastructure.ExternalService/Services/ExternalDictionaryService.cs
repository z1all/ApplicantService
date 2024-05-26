using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;
using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Infrastructure.ExternalService.Configurations;
using Common.Models.Models;

namespace DictionaryService.Infrastructure.ExternalService.Services
{
    public class ExternalDictionaryService : IExternalDictionaryService
    {
        private readonly ILogger<ExternalDictionaryService> _logger;
        private readonly WebExternalOptions _externalOptions;
        private readonly HttpClient _httpClient;

        public ExternalDictionaryService(ILogger<ExternalDictionaryService> logger, HttpClient httpClient, IOptions<WebExternalOptions> externalOptions)
        {
            _logger = logger;
            _externalOptions = externalOptions.Value;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_externalOptions.BaseUrl);

            _httpClient.DefaultRequestHeaders
                .Add(HeaderNames.Accept, "text/plain");

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_externalOptions.Username}:{_externalOptions.Password}");
            _httpClient.DefaultRequestHeaders
                .Add(HeaderNames.Authorization, $"Basic {Convert.ToBase64String(plainTextBytes)}");
        }

        public async Task<ExecutionResult<List<EducationDocumentTypeExternalDTO>>> GetEducationDocumentTypesAsync() =>
             await GetAsync<List<EducationDocumentTypeExternalDTO>>(_externalOptions.DocumentTypeRoute, "GetEducationDocumentsFail");

        public async Task<ExecutionResult<List<EducationLevelExternalDTO>>> GetEducationLevelsAsync() =>
             await GetAsync<List<EducationLevelExternalDTO>>(_externalOptions.EducationLevelRoute, "GetEducationLevelsFail");

        public async Task<ExecutionResult<List<FacultyExternalDTO>>> GetFacultiesAsync() =>
             await GetAsync<List<FacultyExternalDTO>>(_externalOptions.FacultiesRoute, "GetFacultiesFail");

        public async Task<ExecutionResult<List<EducationProgramExternalDTO>>> GetEducationProgramAsync()
        {
            List<EducationProgramExternalDTO> Programs = new();

            try
            {
                int pages = 1;
                for (int i = 0; i < pages; i++)
                {
                    string route = $"{_externalOptions.EducationProgramRoute}?page={i + 1}&size={_externalOptions.EducationProgramCountOnPage}";

                    var programListExternal = await _httpClient.GetFromJsonAsync<ProgramListExternalDTO>(route);
                    if (programListExternal is null)
                    {
                        _logger.LogError($"Unknown error when requesting directory {typeof(ProgramListExternalDTO).Name} from an external service");
                        return new(StatusCodeExecutionResult.InternalServer, "GetEducationProgramFail", error: "Unknown error");
                    }
                    Programs.AddRange(programListExternal.Programs);

                    pages = programListExternal.Pagination.Count;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error when requesting directory {typeof(ProgramListExternalDTO).Name} from an external service");
                throw;
            }

            return new(result: Programs);
        }

        private async Task<ExecutionResult<TResult>> GetAsync<TResult>(string route, string keyError)
        {
            try
            {
                TResult? result
                = await _httpClient.GetFromJsonAsync<TResult>(route);
                if (result is null)
                {
                    _logger.LogError($"Unknown error when requesting directory {typeof(TResult).Name} from an external service");
                    return new(StatusCodeExecutionResult.InternalServer, keyError, error: "Unknown error");
                }

                return new(result: result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error when requesting directory {typeof(TResult).Name} from an external service");
                throw;
            }
        }
    }
}
