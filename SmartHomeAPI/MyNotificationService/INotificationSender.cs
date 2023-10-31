

namespace MyNotificationService;

public interface INotificationSender
{
    public Task SendUserNotificationAsync(string recipientToken, string messageTitle, string messageContent);
}
