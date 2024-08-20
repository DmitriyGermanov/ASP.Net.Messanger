namespace MessagingService.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string? SenderEmail { get; set; }
        public string? ReceiverEmail { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsReceived { get; set; }
    }
}
