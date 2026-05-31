using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuickPay.Data;
using QuickPay.Models.Domain;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;
using System.Security.Claims;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService walletService;

    public WalletController(IWalletService walletService)
    {
        this.walletService = walletService;
    }

    [HttpGet("userid")]   // 👈 clean endpoint
    public async Task<IActionResult> GetWallet()
    {
        

        var userId = int.Parse(
        User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        var result = await walletService.GetBalanceAsync(userId);

        return Ok(new ApiResponeDto<WalletResponeDto>
        {
            Success = true,
            Message = "Wallet retrieved successfully",
            Data = result
        });

    }

  
}