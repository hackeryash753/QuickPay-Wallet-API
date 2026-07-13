using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;

namespace QuickPay.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly INotificationService notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet("Notifications/{pageNumber},{pageSize}")]
        public async Task<IActionResult> GetNotifications(int pageNumber = 1, int pageSize = 1)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result =await notificationService.GetNotificationsAsync(userId,pageNumber,pageSize);

            return Ok(new ApiResponeDto<NotificationResponseDto>
            {
                Success = true,
                Message = "Wallet retrieved successfully",
                Data = result
            });
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult>GetUnreadCount()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await notificationService.GetUnreadCountAsync(userId);

            return Ok(new ApiResponeDto<int>
            {
                Success = true,
                Message = "Wallet retrieved successfully",
                Data = result
            });
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await notificationService.MarkAsReadAsync(id);

            return Ok(new ApiResponeDto<int>
            {
                Success = true,
                Message = "Wallet retrieved successfully"
            });
        }

    }
}
