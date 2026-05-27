using System.ComponentModel.DataAnnotations;

namespace QuickPay.Models.DTO
{
    public class AddMoneyDto
    {
        [Required]
        public int userid { get; set; }

        [Required]
        [Range(1,100000)]
        public decimal amount { get; set; }
    }
}
