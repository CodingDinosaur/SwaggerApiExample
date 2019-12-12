namespace FlowFitExample.Models.Frontend
{
    /// <summary>
    /// Response given when a requested task has started
    /// </summary>
    public class StartMeeseeksTaskResponse
    {
        internal StartMeeseeksTaskResponse(MrMeeseeks meeseeks, string taskType)
        {
            Meeseeks = meeseeks;
            TaskTypeName = taskType;
        }

        /// <summary>
        /// Associated Mr. Meeseeks spawned for this task
        /// </summary>
        public MrMeeseeks Meeseeks { get; set; }
        
        /// <summary>
        /// String representation of the task "type"
        /// <remarks>
        /// This is different than the task type enum, this represents the internal type name of the task.
        /// <br/>In some cases, special tasks have special derived types.
        /// This is largely an internal implementation detail but is provided here for future interopability.
        /// </remarks>
        /// </summary>
        public string TaskTypeName { get; set; }
    }
}
