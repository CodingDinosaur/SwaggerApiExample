using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Models.Frontend
{
    public class MeeseeksTaskStatusDetailed : MeeseeksTaskStatus
    {
        internal MeeseeksTaskStatusDetailed(IMeeseeksTask taskInfo, MrMeeseeks mrMeeseeks) : base(taskInfo)
        {
            MrMeeseeks = mrMeeseeks;
        }

        public MrMeeseeks MrMeeseeks { get; set; }
    }
}
