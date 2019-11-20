using Microsoft.Extensions.Logging;

namespace FlowFitExample.Models
{
    public abstract class BaseMeeseeksTask
    {
        public MeeseeksTaskType TaskType { get; }
        public string Name { get; }
        protected ILogger Log;
        const string DefaultTaskName = "Unnamed Meeseeks Task";

        public BaseMeeseeksTask(MeeseeksTaskType taskType, string name, ILogger log)
        {
            TaskType = taskType;
            Name = !string.IsNullOrWhiteSpace(name) ? name : DefaultTaskName;
            Log = log;
        }

        public abstract void Execute();
    }
}
