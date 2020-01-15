using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <summary>
    /// A typical Meeseeks task
    /// </summary>
    public class GeneralMeeseeksTask : BaseMeeseeksTask
    {
        public GeneralMeeseeksTask(MeeseeksTaskCategory taskCategory, ILogger log, string name = null) : base(taskCategory, log, name) { }

        protected override async Task ExecuteInternalAsync(MrMeeseeks meeseeks)
        {
            Log.LogInformation("I'm Mr. Meeseeks, look at me!");
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
