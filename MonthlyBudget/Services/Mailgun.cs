using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace MonthlyBudget.Services
{
    public class Mailgun
    {
        public static void SendSimpleMessage()
        {
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
            request.AddParameter("from", "Excited User <postmaster@mg.yourmonthlybudget.com>");
            request.AddParameter("to", "crocchio85@gmail.com");
            request.AddParameter("subject", "Hey Cor!");
            request.AddParameter("text", "NBD just got mailgun working!");
            request.Method = Method.POST;

            client.ExecuteAsync(request, response =>
            {
                var content = response.Content;
            });
            /// return shit
        }
    }
}
