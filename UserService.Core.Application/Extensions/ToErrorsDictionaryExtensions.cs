using Microsoft.AspNetCore.Identity;
using System.Collections.Immutable;

namespace UserService.Core.Application.Extensions
{
    public static class ToErrorsDictionaryExtensions
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
    }
}
