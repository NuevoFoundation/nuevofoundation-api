using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Mailer
{
  public class VirtualSessionsMailer
  {
    Mailer _mailer = new Mailer();
    private string _mailerViewsRoot = "./Mailer/MailerViews";
    public VirtualSessionsMailer()
    {
    }

    public async Task SendNewVirtualSessionMail(List<string> volunteerEmails, Guid virtualSessionId)
    {
      MailAddress from = new MailAddress(_mailer.FromEmail);
      MailMessage message = new MailMessage();
      message.From = from;
      foreach(var email in volunteerEmails)
      {
        message.To.Add(email);
      }
      var mailBody = ReadHtmlFile("NewVirtualSession");
      mailBody = mailBody.Replace("<virtualSessionUrl>", $"{Settings.JwtAudience}/virtual-sessions/{virtualSessionId.ToString()}");
      message.Body = mailBody;
      message.BodyEncoding = System.Text.Encoding.UTF8;
      message.Subject = "Nuevo Foundation - Virtual Session Opportunity";
      message.SubjectEncoding = System.Text.Encoding.UTF8;
      message.IsBodyHtml = true;
      await Task.Run(() => _mailer.SendMessageAsync(message));
    }

    private string ReadHtmlFile(string mailName)
    {
      var mailContent = File.ReadAllText($"{_mailerViewsRoot}/{mailName}.html");
      return mailContent;
    }
  }
}
