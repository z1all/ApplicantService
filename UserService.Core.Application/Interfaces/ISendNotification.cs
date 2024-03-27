namespace UserService.Core.Application.Interfaces
{
    public interface ISendNotification
    {
        Task<bool> CreatedApplicant();
        Task<bool> CreatedManager();
        Task<bool> UpdatedUser();
    }
}
