using System;

namespace FlowFitExample.Models
{
    public class StringSegment
    {
        public StringSegment(string language, string value)
        {
            Id = Guid.NewGuid();
            Language = language;
            Value = value;
        }

        public Guid Id { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
        public int Length => Value.Length;
    }
}
