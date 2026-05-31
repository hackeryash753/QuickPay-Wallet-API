using System.ComponentModel.DataAnnotations;

namespace QuickPay.Models.DTO
{
    public class AddMoneyDto
    {
        [Required]
        [Range(1,100000)]
        public decimal amount { get; set; }
    }
}
