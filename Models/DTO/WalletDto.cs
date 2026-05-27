using QuickPay.Models.Domain;

namespace QuickPay.Models.DTO
{
    public class WalletDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public Users Users { get; set; }
    }
}
