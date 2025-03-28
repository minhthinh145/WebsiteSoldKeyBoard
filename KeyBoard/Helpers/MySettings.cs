namespace KeyBoard.Helpers
{
    public static class MySettings
    {
    }
    public static class StattusPayment 
    {
        public const string Pending = "Pending";
        public const string Processing = "Processing";
        public const string Shipped = "Shipped";
        public const string Completed = "Completed";
        public const string Canceled = "Canceled";

        public static readonly List<string> Statuses = new List<string>
        {
            Pending, Processing, Shipped, Completed, Canceled
        };

        public static bool IsValidStatus(string status)
        {
            return Statuses.Contains(status);
        }
    }
}
