using Microsoft.AspNetCore.Identity;
using System.Collections.Immutable;
using Common.Models.Models;

namespace UserService.Infrastructure.Identity.Extensions
{
    public static class IdentityExtensions
    {
        public static ImmutableDictionary<string, List<string>> ToErrorDictionary(this IEnumerable<IdentityError> identityErrors)
        {
            ImmutableDictionary<string, List<string>> errors = ImmutableDictionary<string, List<string>>.Empty;

            foreach (var error in identityErrors)
            {
                errors = errors.Add(error.Code, [error.Description]);
            }

            return errors;
        }

        public static ExecutionResult ToExecutionResultError(this IdentityResult identityResult)
        {
            return new() { Errors = identityResult.Errors.ToErrorDictionary() };
        }

        public static ExecutionResult<T> ToExecutionResultError<T>(this IdentityResult identityResult) where T : class
        {
            return new() { Errors = identityResult.Errors.ToErrorDictionary() };
        }
    }
}
