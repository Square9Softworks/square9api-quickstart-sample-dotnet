using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Square9.QuickstartSample
{

    using Square9Api;
    using Square9Api.Models;
    using Square9Api.Exceptions;

    class Program
    {
        private static int _maxAttempts = 3;

        /// <summary>
        /// Square 9 API Quickstart Sample:
        /// 
        /// When processing volumes of work in the context of a service worker, it is considered best practice to
        /// reuse a longlived token, rather than to request and dispose of a license token for each request. In
        /// simple terms this can reduce each transaction from 3 calls to one, and greatly reduce load on the SQL
        /// database by preventing additional queries necessary by the create license process.
        /// 
        /// Example API Consumer Service Workflow:
        /// 1. Ensure we have a token:
        ///     (a) Load our stored license token (if we have one).
        ///     (b) If we do not have one, request a new one and save it for future use.
        /// 2. Do any work that needs to be completed.
        /// 3. If we are unable to complete work because our license is expired:
        ///     (a) Remove invalid stored token.
        ///     (b) Restart the flow, if we have not failed too many times already.
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            // Load our configuration options.
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var apiConfig = config.GetSection("Square9ApiConfig").Get<ApiConfig>();


            // Ensure we have a token and attempt to complete our "work".
            await GetTokenAndDoWork(apiConfig);
        }

        private static async Task GetTokenAndDoWork(ApiConfig config)
        {
            LimitAttempts(); // Handle repeated failures to prevent any infinite loops.

            try
            {

                // Check for a stored token.
                var token = Storage.Token;

                if (string.IsNullOrEmpty(token))
                {
                    // If no token is stored, request a new one.
                    token = await Licenses.GetTokenAsync(config);

                    // Then store the new token.
                    Storage.Token = token;
                }

                ///// Make any request(s) that require a token.

                // Do any required work with our token.
                await SomeInteractionWithOurApi(config, token);

                ////// Work was completed sucessfully.

                // (optionally) when all work is completed, you can 'deactivate' your license to free the seat to free it for other
                // users/consumers, it's more efficient to leave it active if you are doing more work soon:
                await Licenses.DeactivateTokenAsync(config, token);

                return;
            }
            // Handle assorted failures from our attempted work.
            catch (InvalidApiCredentialsException)
            {
                await ClearStoredTokenAndRetry(config);
            }
            catch (AllLicensesInUseException)
            {
                // All licenses were in use, lets wait a moment before we try again.
                Console.WriteLine("All licenses were in use, waiting 5 seconds before trying again.");
                await Task.Delay(TimeSpan.FromSeconds(5));
                await ClearStoredTokenAndRetry(config);
            }
            catch (TokenExpiredException)
            {
                await ClearStoredTokenAndRetry(config);
            }
        }

        /// <summary>
        /// Remove any stored token if it was not accepted and try again if attempts remain.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static async Task ClearStoredTokenAndRetry(ApiConfig config)
        {
            // Clear the invalid stored token.
            Storage.Token = null;

            // Recurse to obtain a new token and attempt our work again. Decrement the max attempts counter.
            _maxAttempts--;
            await GetTokenAndDoWork(config);
        }

        /// <summary>
        /// Check that we have not exceeded our remaining attempts to perform work due to too many license failures.
        /// </summary>
        private static void LimitAttempts()
        {
            if (_maxAttempts == 0)
            {
                Console.WriteLine("Too many attempts. Exiting.");
                throw new Exception("Max attempts reached.");
            }
        }

        /// <summary>
        /// Some sort of work using our API that requires the use of a license token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task SomeInteractionWithOurApi(ApiConfig config, string token)
        {
            // As long as your license token is valid and a seat is available, it will be accepted and reactivated by the token.
            // You do not need to create a license for each run, this flow significantly speeds up performance by
            // reducing interaction with the API.
            //
            // Just make your calls to our API with your valid license, which you can optionally release from being
            // active when done if you won't be using it again for an extended period. If you will be continuing to use
            // the token signiicantly it is recommended you leave it active to reduce total SQL Server load.

            // For our example we are just going to get a list of the databases with our user.
            // For more documentaion on available API endpoints, check https://square-9.com/api for a guide on REST API Resources.
            var databases = await Databases.GetAllAsync(config, token);
            Console.WriteLine("=== Databases ===");
            databases.ForEach(db => Console.WriteLine($"{db.Id}: {db.Name}"));
            Console.WriteLine("=================");

            Console.WriteLine($"Work was completed with token: {token}. Exiting.");
        }
    }
}
