using System.Collections.Immutable;

namespace UserService.Core.Application.DTOs
{
    public class ErrorResponse
    {
        public string Title { get; set; } = null!;
        public int Status { get; set; }
        public ImmutableDictionary<string, List<string>> Errors { get; set; } = null!;
    }
}