namespace WGMansion.Api.Models
{
    public static class Roles
    {
        public const string Admin = $"Admin";
        public const string Moderator = $"Moderator,{Admin}";
        public const string User = $"User,{Moderator}";
    }
}
