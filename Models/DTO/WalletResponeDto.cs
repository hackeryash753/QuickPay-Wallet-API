namespace QuickPay.Models.DTO
{
    public class WalletResponeDto
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
