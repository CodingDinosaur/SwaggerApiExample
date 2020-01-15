using System;

namespace SwaggerApiExample.Models.Frontend
{
    /// <summary>
    /// Encapsulates generic information about a problem
    /// </summary>
    public class FailureInfo
    {
        internal FailureInfo(string failureMessage, DateTime? encountered = null)
        {
            FailureMessage = failureMessage;
            Encountered = encountered ?? DateTime.Now;
        }

        /// <summary>
        /// Error message
        /// </summary>
        public string FailureMessage { get; set; }

        public DateTime Encountered { get; set; }
    }
}
