using System;

namespace SwaggerApiExample.Models.Meeseeks
{
    /// <summary>
    /// Representation of a Mr. Meeseeks
    /// </summary>
    /// <remarks>
    /// Abstract because you can't have a meeseeks that isn't on a task
    /// </remarks>
    public class MrMeeseeks
    {
        public MrMeeseeks(ulong catchphraseCounter)
        {
            CatchphraseCounter = catchphraseCounter;
            Id = Guid.NewGuid();
            BirthTime = DateTime.Now;
        }

        /// <summary>
        /// Unique ID of this Mr. Meeseeks
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Number of times this Meeseeks has said his catch phrase
        /// </summary>
        public ulong CatchphraseCounter { get; set; }
        
        /// <summary>
        /// Datetime this Meeseeks was summoned
        /// </summary>
        public DateTime BirthTime { get; set; }
        
        /// <summary>
        /// Datetime this Meeseeks poofed
        /// </summary>
        public DateTime? DeathTime { get; set; }

        /// <summary>
        /// Whether or not the Meeseeks is currently alive and working on a task
        /// </summary>
        public bool IsActive => BirthTime < DateTime.Now && !DeathTime.HasValue;
        
        /// <summary>
        /// Whether or not the Meeseeks is likely losing his sanity due to old age
        /// </summary>
        public bool IsLosingSanity => BirthTime < DateTime.Now && DateTime.Now - BirthTime >= TimeSpan.FromDays(2);
    }
}