namespace FlowFitExample.Models
{
    public class DinosaurGenome
    {
        public DinosaurGenome(string commonName, string scienceyName, byte[] topSecretGeneticData)
        {
            CommonName = commonName;
            ScienceyName = scienceyName;
            TopSecretGeneticData = topSecretGeneticData;
        }

        public string CommonName { get; set; }
        public string ScienceyName { get; set; }
        public byte[] TopSecretGeneticData { get; set; }
        public bool WillProbablyEatYouIfCloned { get; } = true;
    }
}
