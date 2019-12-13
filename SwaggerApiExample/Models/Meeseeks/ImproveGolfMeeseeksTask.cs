using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <summary>
    /// A Meeseeks task focused on improving Jerry's golf handicap
    /// </summary>
    public class ImproveGolfMeeseeksTask : BaseMeeseeksTask
    {
        private const string TaskName = "Improve Golf Handicap";
        
        public ImproveGolfMeeseeksTask(ILogger log) 
            : base(MeeseeksTaskCategory.Jerry, TaskName, log) { }

        public override void Execute()
        {
            Log.LogInformation("Keep your shoulders straight!");
        }
    }
}
