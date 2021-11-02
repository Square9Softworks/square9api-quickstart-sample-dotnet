using System;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace Square9.QuickstartSample.Square9Api
{
    using Models;

    public static class Databases
    {
        /// <summary>
        /// Get a license token.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<List<Database>> GetAllAsync(ApiConfig config, string token)
        {
            try
            {
                // Request the list of databases, include our token for authentication.
                using var apiClient = new ApiClient(config, token);
                using var response = await apiClient.GetEndpointAsync("dbs");

                // Check for and handle any non successful HTTP Response codes.
                await ApiClient.HandleFailedResponseCodesAsync(response, "GET Databases");

                // Successfully obtained a list of databases.
                return (await response.Content.ReadFromJsonAsync<DatabaseResponse>()).Databases;
            }
            catch (Exception)
            {
                // (optionally) Handle additional exception failures.
                throw;
            }
        }
    }
}
