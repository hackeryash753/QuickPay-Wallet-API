using QuickPay.Models.DTO;

namespace QuickPay.Services.Interface
{
    public interface IWalletService
    {
        Task<SendMoneyResponseDto> SendMoneyAsync(SendMoneyDto dto);
        Task<AddMoneyResponeDto> AddMoneyAsync(AddMoneyDto dto);

        Task<WalletResponeDto> GetBalanceAsync(int userId);
    }
}
