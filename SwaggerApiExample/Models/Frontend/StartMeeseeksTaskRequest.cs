using System.ComponentModel.DataAnnotations;

namespace SwaggerApiExample.Models.Frontend
{
    /// <summary>
    /// The info for the start of a Meeseeks Request
    /// </summary>
    public class StartMeeseeksTaskRequest
    {
        /// <summary>
        /// Meeseeks task type name (see enum MeeseeksTaskCategory)
        /// </summary>
        [Required]
        public string TaskTypeName { get; set; }
    }
}
