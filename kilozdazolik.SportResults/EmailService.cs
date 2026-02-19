using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _senderEmail;
    private readonly string _senderPassword;
    private readonly bool _enableSsl;

    public EmailService(IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        _smtpServer = emailSettings["SmtpServer"];
        _port = int.Parse(emailSettings["Port"]);
        _senderEmail = emailSettings["SenderEmail"];
        _senderPassword = emailSettings["SenderPassword"];
        _enableSsl = bool.Parse(emailSettings["EnableSsl"]);
    }

    public async Task SendEmail(string to, string subject, string body)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(to);

            using var smtpClient = new SmtpClient(_smtpServer)
            {
                Port = _port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                EnableSsl = _enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

          await smtpClient.SendMailAsync(mailMessage);
          Console.WriteLine("Sending email");
        }
        catch (Exception ex)
        {
            Console.WriteLine("SMTP Error: " + ex.ToString());
            throw new Exception("Error sending email: ", ex);
        }
    }
}