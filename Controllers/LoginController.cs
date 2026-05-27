using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuickPay.Data;
using QuickPay.Models.Domain;
using QuickPay.Models.DTO;
using QuickPay.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuickPay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthService authService;

        public LoginController(AuthService authService)
        {
            this.authService = authService;
        }

        

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await authService.LoginAsync(dto);
            return Ok(new ApiResponeDto<LoginResponeDto>
            {
                Success = true,
                Message = "Login successful",
                Data = result

            });
        }
    }
}
