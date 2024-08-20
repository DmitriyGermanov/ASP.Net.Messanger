namespace MessagingService.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; } 
        public Guid ReceiverId { get; set; } 
        public string Content { get; set; } = String.Empty;
        public DateTime SentAt { get; set; }
        public bool IsReceived { get; set; }
    }
}
