using System.ComponentModel.DataAnnotations;

namespace QuickPay.Models.DTO
{
    public class SendMoneyDto
    {
        [Required]
        public int SenderId { get; set; }
        //public int ReceiverId { get; set; }

        [EmailAddress]
        [Required]
        public string ReceiverEmail { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal Amount { get; set; }
    }
}
