using System.Net.Http.Headers;
using System.Net.Security;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace MKopa.NotificationService.Utils
{
    public interface IRestClientService
    {
        Task<T> PostAsync<T, U>(string url, U payload, IDictionary<string, string> headers, [CallerMemberName] string caller = "");
        Task<T> GetAsync<T>(string url, IDictionary<string, string> headers, [CallerMemberName] string caller = "");
        Task<T> GetAsync<T>(string url, IDictionary<string, string> payload, IDictionary<string, string> headers);
        //Task<T> SEND<T>(string xml, string endpoint, string action, List<KeyValuePair<string, string>> headers, string accept, string contentType) where T : new();
    }

    public class RestClientService : IRestClientService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<RestClientService> _logger;
        public RestClientService(IHttpClientFactory clientFactory, ILogger<RestClientService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        /// <summary>
        /// Generic Async rest client operation with Post method
        /// </summary>
        /// <typeparam name="T">Expected return data type</typeparam>
        /// <typeparam name="U">Data type of Payload</typeparam>
        /// <param name="url">Full absolute URL path</param>
        /// <param name="payload">Paylod body</param>
        /// <param name="headers">Optional - Dictionary of headers</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T, U>(string url, U payload, IDictionary<string, string> headers, [CallerMemberName] string caller = "")
        {
            T objResp = default(T);
            RestResponse response = new();
            try
            {
                var options = new RestClientOptions()
                {
                    //MaxTimeout = -1,
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                };
                var request = new RestRequest(url, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        if (header.Key.Trim() == "Authorization")
                        {
                            options.Authenticator = new JwtAuthenticator(header.Value);
                            request?.AddOrUpdateHeader(header.Key, value: header.Value);
                            continue;
                        }
                        request.AddOrUpdateHeader(header.Key, value: header.Value);
                    }
                }
                var client = new RestClient(options);
                var body = JsonConvert.SerializeObject(payload);
                request.AddStringBody(body, DataFormat.Json);
                response = await client.ExecuteAsync(request);
                _logger.LogInformation($"{url},{response.StatusDescription}: {response.Content}");
                objResp = JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n=====>\n{nameof(PostAsync)} \n Url:::{url} \n {caller} \n Payload:::  {JsonConvert.SerializeObject(payload)}\n HttpResponse::: {JsonConvert.SerializeObject(response)} \n");
                _logger.LogError(ex, $"{nameof(PostAsync)} \n Url:::{url} \n {caller} \n Payload:::  {JsonConvert.SerializeObject(payload)}\n HttpResponse::: {JsonConvert.SerializeObject(response)} \n");
            }
            return objResp;
        }


        public async Task<T> GetAsync<T>(string url, IDictionary<string, string> headers, [CallerMemberName] string caller = "")
        {
            T objResp = default;
            RestResponse response = new();
            try
            {
                var options = new RestClientOptions()
                {
                    //MaxTimeout = -1,
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                };
                var client = new RestClient(options);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        if (header.Key.Trim() == "Authorization")
                        {
                            options.Authenticator = new JwtAuthenticator(header.Value);
                            request.AddHeader(header.Key, value: header.Value);
                            continue;
                        }

                    }
                }
                response = await client.ExecuteAsync(request);
                _logger.LogInformation($"{url}, {response.Content}");
                objResp = JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n=====>\n{nameof(GetAsync)} \n {caller} \n Payload:::  {JsonConvert.SerializeObject(url)}\n HttpResponse::: {JsonConvert.SerializeObject(response)} \n");
                _logger.LogError(ex, $"{nameof(GetAsync)} \n {caller} \n Payload:::  {JsonConvert.SerializeObject(url)}\n HttpResponse::: {JsonConvert.SerializeObject(response)} \n");
            }
            return objResp;
        }

        public async Task<T> GetAsync<T>(string url, IDictionary<string, string> payload, IDictionary<string, string> headers)
        {
            T objResp = default(T);
            try
            {
                var options = new RestClientOptions()
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, value: header.Value);
                    }
                }
                var body = JsonConvert.SerializeObject(payload);

                foreach (KeyValuePair<string, string> itm in payload)
                {
                    request.AddParameter(itm.Key, value: itm.Value);
                }
                //request.AddParameter("bankId", payload);
                RestResponse response = await client.ExecuteAsync(request);

                objResp = JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAsync));
            }
            return objResp;
        }
      
        public T Deserialize<T>(string input) where T : new()
        {
            if (string.IsNullOrEmpty(input))
            {
                return new T();
            }
            else
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));

                using (StringReader sr = new StringReader(input))
                {
                    return (T)ser.Deserialize(sr);
                }

            }
        }

        #region Private Methods
        private HttpClient GetHttpClient(string endpointURL)
        {

            HttpClient client = _clientFactory.CreateClient("General");//new HttpClient(handler);

            try

            {
                client.BaseAddress = new Uri(endpointURL);

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"{MethodBase.GetCurrentMethod().Name}-{ex.Message}");
                throw ex;
            }
            return client;

        }

        private T DeserializeInnerSoapObject<T>(string soapResponse) where T : new()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapResponse);

            var soapBody = xmlDocument.GetElementsByTagName("S:Body")[0];
            string innerObject = soapBody.InnerXml;

            return Deserialize<T>(innerObject);
        }

        #endregion

    }
}
