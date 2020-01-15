using System;
using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Models.Frontend
{
    /// <summary>
    /// Represents status information for a task performed by a Meeseeks
    /// </summary>
    public class MeeseeksTaskStatus
    {
        internal MeeseeksTaskStatus(IMeeseeksTask taskInfo)
        {
            MeeseeksId = taskInfo.AssignedMeeseeks;
            TaskCategory = taskInfo.TaskCategory;
            TaskName = taskInfo.Name;
            HasStarted = true;
        }

        /// <summary>
        /// GUID for the assigned Mr. Meeseeks
        /// </summary>
        public Guid MeeseeksId { get; set; }

        /// <summary>
        /// Task type enum vale
        /// </summary>
        public MeeseeksTaskCategory TaskCategory { get; set; }

        /// <summary>
        /// Basic task information
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// True if the task has started
        /// </summary>
        public bool HasStarted { get; set; }
    }
}
