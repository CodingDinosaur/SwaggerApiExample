using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <summary>
    /// The most fundamental representation of a Meeseeks task
    /// </summary>
    public abstract class BaseMeeseeksTask
    {
        /// <summary>
        /// Task category
        /// </summary>
        public MeeseeksTaskCategory TaskCategory { get; }
        
        /// <summary>
        /// Name of this task
        /// </summary>
        public string Name { get; }
        
        protected ILogger Log;
        const string DefaultTaskName = "Unnamed Meeseeks Task";

        protected BaseMeeseeksTask(MeeseeksTaskCategory taskCategory, string name, ILogger log)
        {
            TaskCategory = taskCategory;
            Name = !string.IsNullOrWhiteSpace(name) ? name : DefaultTaskName;
            Log = log;
        }

        public abstract void Execute();
    }
}
