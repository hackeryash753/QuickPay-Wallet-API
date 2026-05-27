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
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService walletService;

    public WalletController(IWalletService walletService)
    {
        this.walletService = walletService;
    }

    [HttpGet("userid")]   // 👈 clean endpoint
    public async Task<IActionResult> GetWallet(int userid)
    {
        var result = await walletService.GetBalanceAsync(userid);
        return Ok(new ApiResponeDto<WalletResponeDto>
        {
            Success = true,
            Message = "Transaction history retrieved successfully",
            Data = result
        });

    }

  
}