using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Models.Frontend
{
    /// <summary>
    /// Information about the failure to start a meeseeks task
    /// </summary>
    public class TaskStartFailureInfo
    {
        internal TaskStartFailureInfo(string failureMessage, MeeseeksTaskCategory taskCategory = MeeseeksTaskCategory.Unknown)
        {
            FailureMessage = failureMessage;
            TaskCategory = taskCategory;
        }

        /// <summary>
        /// Error message
        /// </summary>
        public string FailureMessage { get; set; }
        /// <summary>
        /// Task type, or unknown if type determination failed
        /// </summary>
        public MeeseeksTaskCategory TaskCategory { get; set; }
    }
}
