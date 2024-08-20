using MessagingService.Models;

namespace MessagingService.Repo
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetMessagesByUser(Guid userId);
        void AddMessage(Message message);
    }
}
