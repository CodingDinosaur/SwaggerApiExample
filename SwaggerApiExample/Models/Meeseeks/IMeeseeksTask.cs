using System;
using System.Threading.Tasks;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <summary>
    /// The most fundamental representation of a Meeseeks task
    /// </summary>
    public interface IMeeseeksTask
    {
        /// <summary>
        /// Task category
        /// </summary>
        MeeseeksTaskCategory TaskCategory { get; }

        /// <summary>
        /// Name of this task
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Whether or not Execute has been called on the task
        /// </summary>
        bool HasStarted { get; }

        /// <summary>
        /// Currently assigned Mr. Meeseeks
        /// </summary>
        Guid AssignedMeeseeks { get; }

        /// <summary>
        /// Initiate the task
        /// </summary>
        Task ExecuteAsync(MrMeeseeks meeseeks);
    }
}