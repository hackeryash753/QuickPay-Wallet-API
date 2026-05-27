using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPay.Data;
using QuickPay.Models.Domain;
using QuickPay.Models.DTO;
using QuickPay.Services;
using System.Security.Cryptography;
using System.Text;

namespace QuickPay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class RegisterController : ControllerBase
    {
        public readonly AuthService _authService;

        public RegisterController(AuthService authService)
        {
            _authService = authService;
        }

        

        [HttpPost]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAysnc(registerDto);
            return Ok(new ApiResponeDto<RegisterResponseDto>
            {
                Success = true,
                Message = "Registration successful",
                Data = result
            });
        }
    }
}
