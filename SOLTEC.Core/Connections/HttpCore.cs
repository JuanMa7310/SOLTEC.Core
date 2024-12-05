using System.Net;
using System.Security.Authentication;
using System.Text;
using Newtonsoft.Json;
using SOLTEC.Core.Exceptions;
using SOLTEC.CoreDTOS;

namespace SOLTEC.Core.Connections
{
    public class HttpCore
    {
        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="headerParameters"></param>
        /// <returns>Returns object of the indicated data type.</returns>
        public virtual async Task<T?> GetAsync<T>(string uri, Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var response = await client.GetAsync(uri);
            ValidateStatusResponse(response);
            var result = response.Content.ReadAsStringAsync().Result;
            ValidateResult(result);
            return JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="headerParameters"></param>
        /// <returns>Returns a list object of the indicated data type.</returns>
        public virtual async Task<IList<T>?> GetAsyncList<T>(string uri, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var response = await client.GetAsync(uri);
            ValidateStatusResponse(response);
            var result = response.Content.ReadAsStringAsync().Result;
            ValidateResult(result);
            return JsonConvert.DeserializeObject<IList<T>>(result);

        }

        /// <summary>
        /// Send a POST request to the specified Uri and parameter as an asynchronous operation.        
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headerParameters"></param>
        /// <returns></returns>
        public virtual async Task PostAsync(string uri, Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var response = await client.PostAsync(uri, default);
            ValidateStatusResponse(response);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <param name="headerParameters"></param>
        /// <returns></returns>
        public virtual async Task PostAsync<T>(string uri, T request, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, 
                "application/json");
            var response = await client.PostAsync(uri, httpContent);
            ValidateStatusResponse(response);

        }

        /// <summary>
        /// Send a POST request to the specified Uri and parameter as an asynchronous operation.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <param name="headerParameters"></param>
        /// <returns>Returns object of the indicated data type (TResult).</returns>
        public virtual async Task<TResult?> PostAsync<T, TResult>(string uri, T request, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, 
                "application/json");
            var response = await client.PostAsync(uri, httpContent);
            ValidateStatusResponse(response);
            var result = response.Content.ReadAsStringAsync().Result;
            ValidateResult(result);
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        /// <summary>
        /// Send a POST request to the specified Uri and parameter as an asynchronous operation.        
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headerParameters"></param>
        /// <returns>Returns object of the indicated data type (TResult).</returns>
        public virtual async Task<TResult?> PostAsync<TResult>(string uri, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var response = await client.PostAsync(uri, default);
            ValidateStatusResponse(response);
            var result = response.Content.ReadAsStringAsync().Result;
            ValidateResult(result);
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri and parameter as an asynchronous operation.        
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headerParameters"></param>
        /// <returns></returns>
        public virtual async Task PutAsync(string uri, Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var response = await client.PutAsync(uri, default);
            ValidateStatusResponse(response);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <param name="headerParameters"></param>
        /// <returns></returns>
        public virtual async Task PutAsync<T>(string uri, T request, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, httpContent);
            ValidateStatusResponse(response);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri and parameter as an asynchronous operation. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <param name="headerParameters"></param>
        /// <returns>Returns object of the indicated data type (TResult).</returns>
        public virtual async Task<TResult?> PutAsync<T, TResult>(string uri, T request, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, httpContent);
            ValidateStatusResponse(response);
            var result = response.Content.ReadAsStringAsync().Result;
            ValidateResult(result);
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri and parameter as an asynchronous operation. 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headerParameters"></param>
        /// <returns>Returns object of the indicated data type (TResult).</returns>
        public virtual async Task<TResult?> PutAsync<TResult>(string uri, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var response = await client.PutAsync(uri, default);
            ValidateStatusResponse(response);
            var result = response.Content.ReadAsStringAsync().Result;
            ValidateResult(result);
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri and parameter as an asynchronous operation. 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="headerParameters"></param>
        /// <returns>Returns object of the indicated data type (TResult).</returns>
        public virtual async Task<TResult?> DeleteAsync<TResult>(string uri, 
            Dictionary<string, string>? headerParameters = null)
        {
            using HttpClient client = new(CreateHttpClientHandler());
            AddHeaders(client, headerParameters);
            var response = await client.DeleteAsync(uri);
            ValidateStatusResponse(response);
            var result = response.Content.ReadAsStringAsync().Result;
            ValidateResult(result);
            return JsonConvert.DeserializeObject<TResult>(result);
        }

        private static void ValidateStatusResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) 
                throw new HttpCoreException(nameof(HttpCoreException),
                    HttpCoreErrorEnum.BadRequest.ToString(),
                    response.StatusCode,
                    response.StatusCode == HttpStatusCode.BadRequest ?
                        response.Content.ReadAsStringAsync().Result :
                        response.ReasonPhrase ?? response.Content.ReadAsStringAsync().Result);
        }

        private static void ValidateResult(string result)
        {
            try
            {
                var problemDetailDto = JsonConvert.DeserializeObject<ProblemDetailsDto>(result);
                if (problemDetailDto?.Status.ToString() == HttpStatusCode.BadRequest.GetHashCode().ToString()) 
                    throw new HttpCoreException(problemDetailDto.Title, problemDetailDto.Type, 
                        HttpStatusCode.BadRequest, problemDetailDto.Detail);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void AddHeaders(HttpClient client, Dictionary<string, string>? headers = null)
        {
            if (headers == null) 
                return;
            foreach (var item in headers)
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
        }

        private static HttpClientHandler CreateHttpClientHandler()
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            clientHandler.SslProtocols |= SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
            return clientHandler;
        }
    }
}
