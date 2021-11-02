using System;
using System.IO;

namespace Square9.QuickstartSample
{
    /// <summary>
    /// A simple token storage class.
    /// 
    /// For the sake of simplicity, this example writes to an unsophisticated local file.
    /// </summary>
    public static class Storage
    {
        /// <summary>
        /// Gets or sets token from storage.
        /// </summary>
        /// <value>Stored token.</value>
        public static string Token
        {
            get
            {
                try
                {
                    // Read the stored token from file.
                    return File.ReadAllText("token");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // Clear stored token when null/empty provided.
                    File.Delete("token");
                }

                // Write a new stored token to file.
                File.WriteAllText("token", value);
            }
        }
    }
}