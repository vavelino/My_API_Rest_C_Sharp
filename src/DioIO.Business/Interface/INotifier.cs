using DevIO.Business.Notifications;
using System.Collections.Generic;

namespace DevIO.Business.Interface
{
    public interface INotifier
    {
        bool IsThereNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
        //Handle maniputar

    }
}
