using DotNetCoreAngular.DAL;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Message> MarkUnreadAsRead(this IQueryable<Message> query, int currentUserId)
        {
            var unreadMessages = query.Where(m => m.DateRead == null
                && m.RecipientId == currentUserId);

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }

            return query;
        }
    }
}
