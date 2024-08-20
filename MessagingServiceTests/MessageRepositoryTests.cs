using MessagingService.Data;
using MessagingService.Models;
using MessagingService.Repo;
using Microsoft.EntityFrameworkCore;

namespace MessagingServiceTests
{
    [TestClass]
    public class MessageRepositoryTests
    {
        private MessageContext? _context;
        private DbContextOptions<MessageContext>? _dbContextOptions;
        private MessageRepository? _messageRepository;

        [TestInitialize]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MessageContext>()
           .UseInMemoryDatabase(databaseName: "UserServiceTestDatabase")
           .Options;

            _context = new MessageContext(_dbContextOptions);
            _messageRepository = new MessageRepository(_context);
            var testMessage1 = GetTestMessage();
            var testMessage2 = GetTestMessage();    

            _context.Add(testMessage1);
            _context.Add(testMessage2);
            _context.SaveChanges();
        }

        [TestMethod]
        public void AddMessage_ShouldAddMessage()
        {
            var message = GetTestMessage();
            _messageRepository?.AddMessage(message);

            var returnMessage = _context?.Messages.FirstOrDefault(message => message.Id == message.Id);

            Assert.IsNotNull(returnMessage);
            Assert.AreEqual(returnMessage.Content, message.Content);
        }

        private Message GetTestMessage() => new()
        {
            SenderId = new Guid(),
            ReceiverId = new Guid(),
            Content = "Тестовое сообщение" + DateTime.Now.ToString(),
            SentAt = DateTime.UtcNow,
            IsReceived = false
        };
    }
}