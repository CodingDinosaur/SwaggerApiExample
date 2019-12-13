namespace SwaggerApiExample.Models
{
    /// <summary>
    /// An odd creature
    /// </summary>
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

        /// <summary>
        /// String identifier for this Platypus
        /// </summary>
        public string Identifier { get; set; }
        
        /// <summary>
        /// Length of the animal's bill
        /// </summary>
        public long BillLength { get; set; }
        
        /// <summary>
        /// Width of the animail's bill
        /// </summary>
        public long BillWidth { get; set; }
        
        /// <summary>
        /// Displacement of the bill if it was submerged in water
        /// </summary>
        public long BillDisplacement { get; set; }
        
        /// <summary>
        /// Universal bill power coeffecient that grants the Platypus its magic
        /// </summary>
        public long BillPowerCoeffecient { get; set; }
    }
}
