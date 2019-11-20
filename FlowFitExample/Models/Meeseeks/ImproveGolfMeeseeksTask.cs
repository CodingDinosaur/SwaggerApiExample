using Microsoft.Extensions.Logging;

namespace FlowFitExample.Models.Meeseeks
{
    public class ImproveGolfMeeseeksTask : BaseMeeseeksTask
    {
        const string TaskName = "Improve Golf Handicap";
        
        public ImproveGolfMeeseeksTask(ILogger log) 
            : base(MeeseeksTaskType.Jerry, TaskName, log) { }

        public override void Execute()
        {
            Log.LogInformation("Keep your shoulders straight!");
        }
    }
}
