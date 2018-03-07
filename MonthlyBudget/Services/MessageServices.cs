using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace MonthlyBudget.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public void SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            var secret = Environment.GetEnvironmentVariable("MAILGUN_KEY");

            RestClient client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3/"),
                Authenticator =
                new HttpBasicAuthenticator("api", secret)
            };

            RestRequest request = new RestRequest();
            request.AddParameter("domain", "mg.yourmonthlybudget.com", ParameterType.UrlSegment);
            request.Resource = "mg.yourmonthlybudget.com/messages";
            request.AddParameter("from", "Your Monthly Budget<postmaster@mg.yourmonthlybudget.com>");
            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);
            request.Method = Method.POST;

            client.ExecuteAsync(request, response =>
            {
                var content = response.Content;
            });
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
