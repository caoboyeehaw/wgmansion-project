namespace WGMansion.Api.Models
{
    public static class Roles
    {
        public const string Admin = $"Admin,{Moderator}";
        public const string Moderator = $"Moderator,{User}";
        public const string User = "User";
    }
}
