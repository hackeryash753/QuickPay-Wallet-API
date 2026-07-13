using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using QuickPay.Data;
using QuickPay.Exceptions;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;

namespace QuickPay.Services
{
    public class NotificationService : INotificationService
    {
        private readonly QuickPayDbContext dbContext;

        public NotificationService(QuickPayDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<NotificationResponseDto> GetNotificationsAsync(int userId,int pageNumber, int pageSize)
        {
            var notifications = await dbContext.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationResponseDto
                {
                    Id = n.Id,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead
                }).FirstOrDefaultAsync();


            if (notifications == null)
            {
                throw new NotFoundException("No notifications found for the specified user.");
            }

            return notifications;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await dbContext.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification =
                await dbContext.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
            {
                throw new NotFoundException(
                    "Notification not found");
            }

            notification.IsRead = true;

            await dbContext.SaveChangesAsync();
        }



    }
}
