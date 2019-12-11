using System.ComponentModel.DataAnnotations;

namespace FlowFitExample.Models.Frontend
{
    /// <summary>
    /// The info for the start of a Meeseeks Request
    /// </summary>
    public class StartMeeseeksTaskRequest
    {
        /// <summary>
        /// Meeseeks task type name (see enum MeeseeksTaskType)
        /// </summary>
        [Required]
        public string TaskTypeName { get; set; }
    }
}
