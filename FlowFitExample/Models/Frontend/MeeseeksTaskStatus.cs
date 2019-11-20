using System;

namespace FlowFitExample.Models.Frontend
{
    public class MeeseeksTaskStatus
    {
        public MeeseeksTaskStatus(Guid meeseeksId, string taskType, BaseMeeseeksTask taskInfo)
        {
            MeeseeksId = meeseeksId;
            TaskType = taskType;
            TaskInfo = taskInfo;
        }

        public Guid MeeseeksId { get; set; }
        public string TaskType { get; set; }
        public BaseMeeseeksTask TaskInfo { get; set; }
    }
}
