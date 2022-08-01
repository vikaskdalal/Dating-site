namespace DotNetCoreAngular.Helpers.Pagination
{
    public class MessageThreadParams
    {
        public int CurrentUserId { get; set; }
        public int RecipientUserId { get; set; }

        public string RecipientUsername { get; set; }

        public int SkipMessages { get; set; }
        public int TakeMessages { get; set; }
    }
}
