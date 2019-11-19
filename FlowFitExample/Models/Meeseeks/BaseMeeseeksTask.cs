using Microsoft.Extensions.Logging;

namespace FlowFitExample.Models
{
    public abstract class BaseMeeseeksTask
    {
        public MeeseeksTaskType TaskType { get; }
        public string Name { get; }
        protected ILogger _log;
        const string DefaultTaskName = "Unnamed Meeseeks Task";

        public BaseMeeseeksTask(MeeseeksTaskType taskType, string name, ILogger log)
        {
            TaskType = taskType;
            Name = !string.IsNullOrWhiteSpace(name) ? name : DefaultTaskName;
            _log = log;
        }

        public abstract void Execute();
    }
}
