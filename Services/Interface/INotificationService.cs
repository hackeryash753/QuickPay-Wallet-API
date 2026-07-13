using QuickPay.Models.DTO;

namespace QuickPay.Services.Interface
{
    public interface INotificationService
    {
        Task<NotificationResponseDto> GetNotificationsAsync(int userId, int pageNumber, int pageSize);

        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
