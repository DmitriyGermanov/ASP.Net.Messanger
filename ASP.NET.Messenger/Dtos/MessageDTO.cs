namespace MessagingService.Dtos
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public string? SenderEmail { get; set; }
        public string? ReceiverEmail { get; set; }
    }
}