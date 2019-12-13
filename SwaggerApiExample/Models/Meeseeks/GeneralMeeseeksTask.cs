using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <summary>
    /// A typical Meeseeks task
    /// </summary>
    public class GeneralMeeseeksTask : BaseMeeseeksTask
    {
        public GeneralMeeseeksTask(MeeseeksTaskCategory taskCategory, string name, ILogger log) : base(taskCategory, name, log) { }

        public override void Execute()
        {
            Log.LogInformation("I'm Mr. Meeseeks, look at me!");
        }
    }
}
