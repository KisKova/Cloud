using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;

namespace MyNotificationService;

public class NotificationSender : INotificationSender
{
    public async Task SendUserNotificationAsync(string recipientToken, string notificationTitle, string notificationContent)
    {
        var pushMessage = new Message()
        {
            Notification = new Notification
            {
                Title = notificationTitle,
                Body = notificationContent
            },

            Token = recipientToken,
        };

        var messagingClient = FirebaseMessaging.GetMessaging(FirebaseApp.DefaultInstance);
        var sendResult = await messagingClient.SendAsync(pushMessage);
        Console.WriteLine(sendResult);
    }
}
