using QuickPay.Models.Domain;
using QuickPay.Models.DTO;

namespace QuickPay.Services.Interface
{
    public interface IAuthService
    {
        string GenerateJwtToken(Users user);
        Task<RegisterResponseDto> RegisterAysnc(RegisterDto registerDto);

        Task<LoginResponeDto> LoginAsync(LoginDto loginDto);

    }
         
}
