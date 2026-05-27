using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPay.Data;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;

namespace QuickPay.Controllers
{
    [ApiController]
    public class AddMoneyController : ControllerBase
    {
        public readonly IWalletService walletService;
        public AddMoneyController(IWalletService walletService)
        {
           this.walletService = walletService;   
        }


        [HttpPost("AddMoney")]
        public async Task<IActionResult> AddMoney(AddMoneyDto addMoneyDto)
        {
           var result = await walletService.AddMoneyAsync(addMoneyDto);
            
            return Ok(new ApiResponeDto<AddMoneyResponeDto>
            { 
                 Success = true,
                 Message = "Money added successfully",
                 Data = result

            });

        }
    }
}
