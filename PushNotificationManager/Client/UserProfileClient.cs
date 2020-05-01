using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PushNotification.Manager
{
    public class UserProfileClient : IUserProfileClient
    {
        IConfiguration configuration;
        const string UserProfileBaseUrlKey = "UserProfileBaseUrl";
        public UserProfileClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IEnumerable<string>> GetGCMClientIdsByUserIds(IEnumerable<Guid>userIds)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>(UserProfileBaseUrlKey));
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                //HTTP GET
                var response = await client.GetAsync("users/gcmids");

                if (response.IsSuccessStatusCode)
                {
                    string stringData = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(stringData))
                    {
                        return null;
                    }

                    return JsonConvert.DeserializeObject<IEnumerable<string>>(stringData);
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw new Exception($"Call to the service {response.RequestMessage.RequestUri} " +
                    $"failed with the status code : {response.StatusCode} and reason phrase :{ response.ReasonPhrase}");

            }
        }
    }
}
