namespace UserService.Core.Application.Interfaces
{
    public interface ITokenDbService
    {
        Task<bool> SaveTokensAsync(string refreshToken, Guid accessTokenJTI);
        Task<bool> RemoveTokensAsync(Guid accessTokenJTI);
        Task<bool> TokensExist(string refresh, Guid accessTokenJTI);
    }
}
