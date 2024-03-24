namespace UserService.Core.Application.Interfaces
{
    public interface ITokenDbService
    {
        Task<bool> SaveTokens(string refreshToken, Guid accessTokenJTI);
    }
}
