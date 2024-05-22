using Common.Models.Enums;

namespace Common.Models.Enums
{
    public static class Role
    {
        public const string Applicant = "Applicant";
        public const string Manager = "Manager";
        public const string MainManager = "MainManager";
        public const string Admin = "Admin";
    }
}

public static class RoleExtensions
{
    public static string ToRu(this string role)
    {
        return role switch
        {
            Role.Admin => "Администратор",
            Role.MainManager => "Главный менеджер",
            Role.Manager => "Менеджер",
            Role.Applicant => "Абитуриент",
            _ => throw new InvalidDataException()
        };
    }
}