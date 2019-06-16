using System;
namespace API.Interfaces
{
  public interface INotification
  {
    // TODO: leverage this interface to send notifcations over other
    // platforms versus only email - e.g. Teams
    void SendNewVirtualSessionsNotification(string message);
  }
}
