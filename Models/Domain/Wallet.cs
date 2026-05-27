using System.ComponentModel.DataAnnotations;

namespace QuickPay.Models.Domain
{
    public class Wallet
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
