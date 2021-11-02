using System;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Square9.QuickstartSample.Square9Api
{
    using Models;

    public static class Licenses
    {

        /// <summary>
        /// Get a license token.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<string> GetTokenAsync(ApiConfig config)
        {
            try
            {
                // Request a license token.
                using var apiClient = new ApiClient(config);
                using var response = await apiClient.GetEndpointAsync("licenses?storeCredentials=true");

                // Check for and handle any non successful HTTP Response codes.
                await ApiClient.HandleFailedResponseCodesAsync(response, "GET License");

                // Successfully obtained a token.
                return (await response.Content.ReadFromJsonAsync<LicenseToken>()).Token;
            }
            catch (Exception)
            {
                // (optionally) Handle additional exception failures.
                throw;
            }
        }

        /// <summary>
        /// Get a license token.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task DeactivateTokenAsync(ApiConfig config, string token)
        {
            try
            {
                // Request a license token.
                using var apiClient = new ApiClient(config);
                using var response = await apiClient.DeleteEndpointAsync($"licenses/{token}");

                // Check for and handle any non successful HTTP Response codes.
                await ApiClient.HandleFailedResponseCodesAsync(response, "DELETE License");

                // Successfully deactivated.
                return;
            }
            catch (Exception)
            {
                // (optionally) Handle additional exception failures.
                throw;
            }
        }
    }
}
