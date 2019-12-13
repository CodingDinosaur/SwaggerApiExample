using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <summary>
    /// A Meeseeks task focused on improving pinball scoress
    /// </summary>
    public class ImprovePinballMeeseeksTask : BaseMeeseeksTask
    {
        private const string TaskName = "Improve Pinball Score";

        public ImprovePinballMeeseeksTask(ILogger log) 
            : base(MeeseeksTaskCategory.Simple, TaskName, log) { }

        public override async Task ExecuteAsync()
        {
            Log.LogInformation("Hey look at me, I'm Mr. Meeseeks and we're at Blips & Chitz!");
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
