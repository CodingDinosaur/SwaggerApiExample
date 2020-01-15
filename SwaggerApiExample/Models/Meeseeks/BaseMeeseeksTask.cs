using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <inheritdoc cref="IMeeseeksTask"/>
    public abstract class BaseMeeseeksTask : IMeeseeksTask
    {
        /// <inheritdoc/>
        public MeeseeksTaskCategory TaskCategory { get; }
        
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public bool HasStarted { get; private set; }

        /// <inheritdoc/>
        public Guid AssignedMeeseeks { get; private set; }

        protected ILogger Log;
        private const string DefaultTaskName = "Unnamed Meeseeks Task";

        protected BaseMeeseeksTask(MeeseeksTaskCategory taskCategory, ILogger log, string name = null)
        {
            TaskCategory = taskCategory;
            Name = !string.IsNullOrWhiteSpace(name) ? name : DefaultTaskName;
            Log = log;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(MrMeeseeks meeseeks)
        {
            HasStarted = true;
            AssignedMeeseeks = meeseeks.Id;
            await ExecuteInternalAsync(meeseeks);
        }

        /// <summary>
        /// Internal implementation for specific Meeseeks tasks
        /// </summary>
        /// <returns></returns>
        protected abstract Task ExecuteInternalAsync(MrMeeseeks meeseeks);
    }
}
