using System;

namespace FlowFitExample.Models
{
    /// <summary>
    /// A block of text with localization information
    /// </summary>
    public class StringSegment
    {
        public StringSegment(string language, string value)
        {
            Id = Guid.NewGuid();
            Language = language;
            Value = value;
        }

        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Language of the text
        /// </summary>
        public string Language { get; set; }
        
        /// <summary>
        /// Underlying text value
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// Length of the text value
        /// </summary>
        public int Length => Value.Length;
    }
}
