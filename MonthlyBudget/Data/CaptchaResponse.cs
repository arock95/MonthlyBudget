using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace MonthlyBudget.Data
{
    public class CaptchaResponse
    {
        public static bool Validate(string EncodedResponse)
        {
            var secret = Environment.GetEnvironmentVariable("RECAPTCHA_SECRET");
            HttpClient client = new System.Net.Http.HttpClient
            {
                BaseAddress = new Uri("https://www.google.com")
            };
            Dictionary<string, string> content = new Dictionary<string, string>
                {
                    { "secret", secret },
                    { "response", EncodedResponse }
                };
            
            FormUrlEncodedContent postShit = new FormUrlEncodedContent(content);
            HttpResponseMessage response = client.PostAsync("recaptcha/api/siteverify", postShit).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var formattedContent = JsonConvert.DeserializeObject<CaptchaResponse>(responseContent);
            var success = Boolean.Parse(formattedContent.Success);
            return success;
        }

        [JsonProperty("success")]
        public string Success { get; set; }
    }
}
