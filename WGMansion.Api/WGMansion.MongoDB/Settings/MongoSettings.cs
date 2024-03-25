namespace WGMansion.MongoDB.Settings
{
    public class MongoSettings
    {
        public required string Account { get; set; }
        public required string Password { get; set; }
        public required string Database { get; set; }
        public required string Collection { get; set; }
        public required string Url { get; set; }
    }
}
