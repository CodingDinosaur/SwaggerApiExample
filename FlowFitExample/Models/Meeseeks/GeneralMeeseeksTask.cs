using Microsoft.Extensions.Logging;

namespace FlowFitExample.Models.Meeseeks
{
    public class GeneralMeeseeksTask : BaseMeeseeksTask
    {
        public GeneralMeeseeksTask(MeeseeksTaskType taskType, string name, ILogger log) : base(taskType, name, log) { }

        public override void Execute()
        {
            _log.LogInformation("I'm Mr. Meeseeks, look at me!");
        }
    }
}
