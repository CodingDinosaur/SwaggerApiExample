using Microsoft.Extensions.Logging;

namespace FlowFitExample.Models.Meeseeks
{
    public class ImprovePinballMeeseeksTask : BaseMeeseeksTask
    {
        private const string TaskName = "Improve Pinball Score";

        public ImprovePinballMeeseeksTask(ILogger log) 
            : base(MeeseeksTaskType.Simple, TaskName, log) { }

        public override void Execute()
        {
            Log.LogInformation("Hey look at me, I'm Mr. Meeseeks and we're at Blips & Chitz!");
        }
    }
}
