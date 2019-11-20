namespace FlowFitExample.Models.Frontend
{
    public class StartMeeseeksTaskResponse
    {
        public StartMeeseeksTaskResponse(bool success, MrMeeseeks meeseeks, string taskType)
        {
            Success = success;
            Meeseeks = meeseeks;
            TaskType = taskType;
        }

        public bool Success { get; set; }
        public MrMeeseeks Meeseeks { get; set; }
        public string TaskType { get; set; }
    }
}
