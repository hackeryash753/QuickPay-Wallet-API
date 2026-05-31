namespace QuickPay.Models.DTO
{
    public class TransactionHistoryDto
    {
        public decimal Amount { get; set; } 
        public string Type { get; set; } 

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public int SenderWalletId { get; set; }

        public int ReceiverWalletId { get; set; }

        public string Direction { get; set; }


    }
}
