using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Manager
{
    public class UserProfileClient : IUserProfileClient
    {
        IConfiguration configuration;     
        public UserProfileClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IEnumerable<string>> GetGCMClientIdsByUserIds(IEnumerable<Guid> userIds)
        {
            using (var client = new HttpClient())
            {
             
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                //HTTP GET

                StringBuilder queryParamsBr = new StringBuilder("?");
                userIds.ToList().ForEach(userid => queryParamsBr.Append($"userIds={userid}&"));
                var queryParams = queryParamsBr.ToString().TrimEnd('&');

                var uri = configuration.GetValue<string>("UserProfileBaseUrl") + "/users/users/gmcclientid" + queryParams;


                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var stringData = await response.Content.ReadAsStringAsync();
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
