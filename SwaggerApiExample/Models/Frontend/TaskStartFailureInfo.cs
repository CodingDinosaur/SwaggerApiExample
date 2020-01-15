using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Models.Frontend
{
    /// <summary>
    /// Information about the failure to start a meeseeks task
    /// </summary>
    public class TaskStartFailureInfo : FailureInfo
    {
        internal TaskStartFailureInfo(string failureMessage, MeeseeksTaskCategory taskCategory = MeeseeksTaskCategory.Unknown)
         : base(failureMessage)
        {
            TaskCategory = taskCategory;
        }

        /// <summary>
        /// Task type, or unknown if type determination failed
        /// </summary>
        public MeeseeksTaskCategory TaskCategory { get; set; }
    }
}
