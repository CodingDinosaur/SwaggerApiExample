namespace FlowFitExample.Models
{
    public class Platypus
    {
        public Platypus(string identifier, long billLength, long billWidth, long billDisplacement, long billPowerCoeffecient)
        {
            Identifier = identifier;
            BillLength = billLength;
            BillWidth = billWidth;
            BillDisplacement = billDisplacement;
            BillPowerCoeffecient = billPowerCoeffecient;
        }

        public string Identifier { get; set; }
        public long BillLength { get; set; }
        public long BillWidth { get; set; }
        public long BillDisplacement { get; set; }
        public long BillPowerCoeffecient { get; set; }
    }
}
