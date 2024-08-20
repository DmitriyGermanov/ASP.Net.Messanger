namespace MessagingService.Dtos
{
    public class SendMessageRequestDTO
    {
        public Guid ReceiverId { get; set; }
        public string Content { get; set; } = String.Empty;
    }
}
