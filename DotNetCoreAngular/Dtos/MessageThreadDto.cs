using DotNetCoreAngular.Helpers.Pagination;

namespace DotNetCoreAngular.Dtos
{
    public class MessageThreadDto
    {
        public MessageThreadDto(int totalMessages, int messageLoaded, IEnumerable<MessageDto> messages)
        {
            TrackMessageThread = new TrackMessageThread(totalMessages, messageLoaded);
            Messages = new List<MessageDto>(messages);
        }

        public List<MessageDto> Messages { get; set; }

        public TrackMessageThread TrackMessageThread { get; set; }
    }

    public class TrackMessageThread
    {
        public TrackMessageThread(int totalMessages, int messageLoaded)
        {
            TotalMessages = totalMessages;
            MessageLoaded = messageLoaded;
        }
        public int TotalMessages { get; set; }

        public int MessageLoaded { get; set; }
        public string? FriendUsername { get; set; }
    }
}
