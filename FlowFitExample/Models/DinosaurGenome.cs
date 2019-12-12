using System;
namespace FlowFitExample.Models
{
    /// <summary>
    /// Represents a Dinosaur with a unique gene sequence
    /// </summary>
    public class DinosaurGenome
    {
        public DinosaurGenome(string commonName, string scienceyName, Span<byte> topSecretGeneticData)
        {
            CommonName = commonName;
            ScienceyName = scienceyName;
            TopSecretGeneticData = topSecretGeneticData.ToArray();
        }

        /// <summary>
        /// Simple / common name
        /// </summary>
        public string CommonName { get; set; }
        
        /// <summary>
        /// Fancy name
        /// </summary>
        public string ScienceyName { get; set; }
        
        /// <summary>
        /// Dinosaur genome data
        /// </summary>
        public byte[] TopSecretGeneticData { get; set; }
        
        /// <summary>
        /// Whether or not the Dinosaur will probably eat you if you clone it.  Always true.
        /// </summary>
        public bool WillProbablyEatYouIfCloned { get; } = true;
    }
}
