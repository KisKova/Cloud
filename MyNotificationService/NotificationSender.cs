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
        Console.WriteLine("We are inside the Sending function.");
        var pushMessage = new Message()
        {
            Notification = new Notification
            {
                Title = notificationTitle,
                Body = notificationContent
            },

            Token = recipientToken,
        };

        Console.WriteLine("This is before the FirebaseMessaging.");
        var messagingClient = FirebaseMessaging.GetMessaging(FirebaseApp.DefaultInstance);
        Console.WriteLine("This is after the FirebaseMessaging.");
        var sendResult = await messagingClient.SendAsync(pushMessage);
        Console.WriteLine("I'm not sure where is the problem.");
        Console.WriteLine(sendResult);
    }
}
