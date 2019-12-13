using System;
using System.Threading.Tasks;
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

        public override async Task ExecuteAsync()
        {
            Log.LogInformation("Keep your shoulders straight!");
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
