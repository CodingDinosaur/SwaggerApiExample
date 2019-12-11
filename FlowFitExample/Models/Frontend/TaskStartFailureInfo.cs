namespace FlowFitExample.Models.Frontend
{
    /// <summary>
    /// Information about the failure to start a meeseeks task
    /// </summary>
    public class TaskStartFailureInfo
    {
        public TaskStartFailureInfo(string failureMessage, MeeseeksTaskType taskType = MeeseeksTaskType.Unknown)
        {
            FailureMessage = failureMessage;
            TaskType = taskType;
        }

        /// <summary>
        /// Error message
        /// </summary>
        public string FailureMessage { get; set; }
        /// <summary>
        /// Task type, or unknown if type determination failed
        /// </summary>
        public MeeseeksTaskType TaskType { get; set; }
    }
}
