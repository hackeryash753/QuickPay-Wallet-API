namespace QuickPay.Models.DTO
{
    public class SendMoneyResponseDto
    {

        public int SenderWalletId { get; set; }
        public int ReceiverWalletId { get; set; }
        public decimal Amount { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
