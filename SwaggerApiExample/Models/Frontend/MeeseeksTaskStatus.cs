using System;
using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Models.Frontend
{
    /// <summary>
    /// Represents status information for a task performed by a Meeseeks
    /// </summary>
    public class MeeseeksTaskStatus
    {
        internal MeeseeksTaskStatus(Guid meeseeksId, MeeseeksTaskCategory taskCategory, BaseMeeseeksTask taskInfo)
        {
            MeeseeksId = meeseeksId;
            TaskCategory = taskCategory;
            TaskInfo = taskInfo;
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
        public BaseMeeseeksTask TaskInfo { get; set; }
    }
}
