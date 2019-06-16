using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Mailer
{
  public class Mailer
  {
    private SmtpClient _client;
    public string FromEmail;

    public Mailer()
    {
      FromEmail = Settings.Email;
      _client = new SmtpClient
      {
        Host = "smtp.live.com",
        Port = 587, 
        EnableSsl = true,
        Credentials = new NetworkCredential(Settings.Email, Settings.EmailPassword)
      };
    }

    public async Task SendMessageAsync(MailMessage message)
    {
      await Task.Run(() => _client.SendAsync(message, null));
    }
  }
}
