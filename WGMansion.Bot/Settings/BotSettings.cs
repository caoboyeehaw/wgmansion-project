namespace WGMansion.Bot.Settings
{
    public class BotSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int MarketOrderChance { get; set; }
        public float LimitMargin { get; set; }
        public int MaxOrders { get; set; }
        public int CycleRateSeconds { get; set; }
    }
}
