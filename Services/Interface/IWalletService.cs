using QuickPay.Models.DTO;

namespace QuickPay.Services.Interface
{
    public interface IWalletService
    {
        Task<SendMoneyResponseDto> SendMoneyAsync(SendMoneyDto dto, int senderId);
        Task<AddMoneyResponeDto> AddMoneyAsync(AddMoneyDto dto,int userId);

        Task<WalletResponeDto> GetBalanceAsync(int userId);
    }
}
