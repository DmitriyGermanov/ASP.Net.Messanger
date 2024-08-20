using MessagingService.Data;
using MessagingService.Models;

namespace MessagingService.Repo
{
    public class MessageRepository(MessageContext messageContext) : IMessageRepository
    {
        private readonly MessageContext _context = messageContext;
        public void AddMessage(Message message)
        {
            try
            {
                _context.Add(message);
                _context.SaveChanges();
            }
            catch { throw; }
        }

        public IEnumerable<Message> GetMessagesByUser(Guid userId)
        {
            var messages = _context.Messages
                .Where(m => m.ReceiverId == userId && !m.IsReceived)
                .ToList();

            messages.ForEach(m => m.IsReceived = true);

            _context.SaveChanges();

            return messages;
        }
    }
}
