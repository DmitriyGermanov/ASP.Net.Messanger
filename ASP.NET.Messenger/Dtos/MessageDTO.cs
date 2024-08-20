namespace MessagingService.Dtos
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = String.Empty; 
        public DateTime SentAt { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}