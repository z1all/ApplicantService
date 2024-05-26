using Common.Models.DTOs;

namespace LoggerService.Core.Application.DTOs
{
    public class LogPagedDTO
    {
        public required List<LogInfoDTO> Logs { get; set; }
        public required PageInfoDTO Pagination { get; set; }
    }
}
