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
            
            testMessage2.ReceiverId = testMessage1.ReceiverId;

            _context.Add(testMessage1);
            _context.Add(testMessage2);
            _context.SaveChanges();
        }

        [TestMethod]
        public void AddMessage_ShouldAddMessage()
        {
            var message = GetTestMessage();
            _messageRepository?.AddMessage(message);

            var returnMessage = _context?.Messages.FirstOrDefault(mess => mess.Id == message.Id);

            Assert.IsNotNull(returnMessage);
            Assert.AreEqual(returnMessage.Content, message.Content);
        }

        [TestMethod]
        public void GetMessageByUser_ShouldReturnMessages()
        {
            if (_context == null)
                Assert.Fail("_context can't be Null.");
            if (_messageRepository == null)
                Assert.Fail("_messageRepository can't be Null.");

            var receiver = _context.Messages.Select(message => message.ReceiverId).First();
            var messages = _messageRepository.GetMessagesByUser(receiver);

            Assert.IsNotNull(messages);
        }

        private Message GetTestMessage() => new()
        {
            Id = Guid.NewGuid(),
            SenderId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            Content = "Тестовое сообщение" + DateTime.Now.ToString(),
            SentAt = DateTime.UtcNow,
            IsReceived = false
        };
    }
}