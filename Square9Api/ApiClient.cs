using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Square9.QuickstartSample.Square9Api
{
    using Models;
    using Exceptions;

    public class ApiClient : IDisposable
    {
        /// <summary>
        /// HttpClient:
        /// 
        /// As a bit of a warning, if lots of the instance may be created in a long running process. There are possible
        /// issues of socket exhaustion despite Microsoft documenting the disposiblity of the HttpClient as safe. In
        /// this case we will just use a static instance and deliberately skip disposing it.
        /// </summary>
        /// <returns></returns>
        private static readonly HttpClient _client = new HttpClient();
        private ApiConfig config;

        public ApiClient(ApiConfig apiConfig, string token = null)
        {
            this.config = apiConfig;

            // If we don't have a token for this call, we will provide our authentication credentials instead.
            if (string.IsNullOrEmpty(token))
            {
                // Provide credentials for the request.
                SetAuthHeader();
            }
            else
            {
                // Add our authorized token header.
                //
                // If you are performing requests against a token with stored credentials, the required identity can be
                // determined from your token, and credentials  are not required. This can be useful to you in
                // situations where you are developing an application that you only want to have an "API Key" to
                // perform work as a user, but do not want to (or have) the users credentials.
                SetTokenHeader(token);
            }

        }

        /// <summary>
        /// Set an AUTH-TOKEN header on the request with our license token.
        /// </summary>
        /// <param name="token"></param>
        private void SetTokenHeader(string token)
        {
            _client.DefaultRequestHeaders.Add("AUTH-TOKEN", token);
        }

        /// <summary>
        /// Set a Basic Authentication header to the request to provide authorization.
        /// </summary>
        /// <param name="config"></param>
        private void SetAuthHeader()
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.Username}:{config.Password}")));
        }

        /// <summary>
        /// GET response from a Square 9 API endpoint.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetEndpointAsync(string endpoint)
        {
            return await _client.GetAsync($"{config.BaseUrl}/{endpoint}");
        }

        /// <summary>
        /// DELETE a Square 9 API endpoint resource.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteEndpointAsync(string endpoint)
        {
            return await _client.DeleteAsync($"{config.BaseUrl}/{endpoint}");
        }

        public void Dispose()
        {
            // We do NOT dispose our singleton client instance. We will just keep updating its settings for this example,
            // However your implementation and preference may vary since concurrency could create issues if you just
            // copied this code.
            // _client.Dispose();
        }

        public static async Task HandleFailedResponseCodesAsync(HttpResponseMessage response, string operationDescription)
        {
            if (!response.IsSuccessStatusCode)
            {
                // An error occured obtaining a license.
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Forbidden:
                        throw new InvalidApiCredentialsException();
                    case System.Net.HttpStatusCode.BadRequest:
                        var content = await response.Content.ReadAsStringAsync();
                        if (content.Contains("All licenses are in use"))
                        {
                            throw new AllLicensesInUseException();
                        }
                        goto default; // fall through to default for any other "Bad Request"
                    default:
                        throw new Exception($"Unable to {operationDescription}, HTTP response phrase: ${response.ReasonPhrase}");
                }
            }
        }
    }
}
