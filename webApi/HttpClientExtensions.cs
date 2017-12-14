using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace webApi
{
     public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public static class HttpClientExtension
    {
        public const string ServiceUnavailableMessage = "Internal Service is not available. Check logs for more details.";
        public static async Task<HttpResponseMessage> GetAsync(this string url, ILogger logger = null, string bearerToken = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(bearerToken))
                    {
                        client.SetBearerToken(bearerToken);
                    }
                    return await client.GetAsync(url);
                }
            }
            catch (HttpRequestException requestException)
            {
                logger?.LogWarning(new EventId(), requestException, $"{ServiceUnavailableMessage}. URL: {url}");
                throw new ServiceUnavailableException(ServiceUnavailableMessage,
                    requestException);
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(this string url, IDictionary<string, string> qs, ILogger logger = null, string bearerToken = null)
        {
            return await $"{url}?{string.Join("&", qs.Select(x => $"{x.Key}={x.Value}"))}".GetAsync(logger, bearerToken);
        }

        public static async Task<T> GetAsync<T>(this string url, IDictionary<string, string> qs, ILogger logger = null, string bearerToken = null)
        {
            var result = await GetAsync(url, qs, logger);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result);
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(this string url, T body, ILogger logger = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    return await client.PostAsync(url, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
                }
            }
            catch (HttpRequestException requestException)
            {
                logger?.LogWarning(new EventId(), requestException, $"{ServiceUnavailableMessage}. URL: {url}");
                throw new ServiceUnavailableException(ServiceUnavailableMessage, requestException);
            }
        }

        public static async Task<HttpResponseMessage> PutAsync<T>(this string url, T body, ILogger logger = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    return await client.PutAsync(url,
                        new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body), Encoding.UTF8,
                            "application/json"));
                }
            }
            catch (HttpRequestException requestException)
            {
                logger?.LogWarning(new EventId(), requestException, $"{ServiceUnavailableMessage}. URL: {url}");
                throw new ServiceUnavailableException(ServiceUnavailableMessage,
                    requestException);
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(this string url, HttpContent requestContent = null, string bearerToken = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    client.SetBearerToken(bearerToken);
                }
                return await client.PostAsync(url, requestContent);
            }
        }
    }
}
