using System;

namespace FlowFitExample.Models
{
    public abstract class MrMeeseeks
    {
        public Guid Id { get; set; }
        public ulong CatchphraseCounter { get; set; }
        public DateTime BirthTime { get; set; }
        public DateTime? DeathTime { get; set; }
        public BaseMeeseeksTask CurrentTask { get; set; }
        public bool IsActive => BirthTime < DateTime.Now && !DeathTime.HasValue && CurrentTask != null;
        public bool IsLosingSanity => BirthTime < DateTime.Now && DateTime.Now - BirthTime >= TimeSpan.FromDays(2);
    }

    public class MrMeeseeks<TTask> : MrMeeseeks where TTask : BaseMeeseeksTask
    {
        public MrMeeseeks(ulong catchphraseCounter, TTask currentTask = null)
        {
            Id = Guid.NewGuid();
            CatchphraseCounter = catchphraseCounter;
            base.CurrentTask = currentTask;
            BirthTime = DateTime.Now;
        }

        public new TTask CurrentTask => base.CurrentTask as TTask;
    }
}
