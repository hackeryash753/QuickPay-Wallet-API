namespace QuickPay.Models.DTO
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }
    }
}
