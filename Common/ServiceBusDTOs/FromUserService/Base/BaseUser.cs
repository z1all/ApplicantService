namespace Common.ServiceBusDTOs.FromUserService.Base
{ 
    public abstract class BaseUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
