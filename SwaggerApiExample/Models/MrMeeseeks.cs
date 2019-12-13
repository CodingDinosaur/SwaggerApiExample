using System;
using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Models
{
    /// <summary>
    /// Representation of a Mr. Meeseeks
    /// </summary>
    /// <remarks>
    /// Abstract because you can't have a meeseeks that isn't on a task
    /// </remarks>
    public abstract class MrMeeseeks
    {
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
        /// Task this Meeseeks is executing
        /// </summary>
        public BaseMeeseeksTask CurrentTask { get; set; }
        
        /// <summary>
        /// Whether or not the Meeseeks is currently alive and working on a task
        /// </summary>
        public bool IsActive => BirthTime < DateTime.Now && !DeathTime.HasValue && CurrentTask != null;
        
        /// <summary>
        /// Whether or not the Meeseeks is likely losing his sanity due to old age
        /// </summary>
        public bool IsLosingSanity => BirthTime < DateTime.Now && DateTime.Now - BirthTime >= TimeSpan.FromDays(2);
    }

    /// <inheritdoc cref="MrMeeseeks"/>
    public class MrMeeseeks<TTask> : MrMeeseeks where TTask : BaseMeeseeksTask
    {
        public MrMeeseeks(ulong catchphraseCounter, TTask currentTask = null)
        {
            Id = Guid.NewGuid();
            CatchphraseCounter = catchphraseCounter;
            base.CurrentTask = currentTask;
            BirthTime = DateTime.Now;
        }

        /// <inheritdoc cref="MrMeeseeks.CurrentTask"/>
        public new TTask CurrentTask => base.CurrentTask as TTask;
    }
}
