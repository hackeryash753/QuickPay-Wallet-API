using QuickPay.Models.Domain;

public class Transactions
{
    public int Id { get; set; }

    public int SenderWalletId { get; set; }
    public Wallet SenderWallet { get; set; }

    public int ReceiverWalletId { get; set; }
    public Wallet ReceiverWallet { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }
}