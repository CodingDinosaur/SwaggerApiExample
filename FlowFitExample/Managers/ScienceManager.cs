using System.Collections.Generic;
using System.Linq;
using FlowFitExample.Models;

namespace FlowFitExample.Managers
{
    public class ScienceManager : IScienceManager
    {
        private readonly List<DinosaurGenome> _dinosaurs = new List<DinosaurGenome>();
        private List<Platypus> _platypuses = new List<Platypus>();

        private List<StringSegment> _scienceyWords = new List<StringSegment>
        {
            new StringSegment("enUS", "Tyrannosaurus Rex"),
            new StringSegment("frFR", "Le Tyrannosaurus Rex"),
            new StringSegment("esMX", "El Tyrannosaurus Rex"),
            new StringSegment("itIT", "Eugotta Runnawayo"),
        };

        public DinosaurGenome RegisterNewDinosaurGenome(string name, byte[] geneticData)
        {
            var newGenome = new DinosaurGenome(name, name + " (but fancy)", geneticData);
            _dinosaurs.Add(newGenome);
            return newGenome;
        }

        public DinosaurGenome GetDinosaurGenomeByName(string name)
        {
            return _dinosaurs.FirstOrDefault(d => d.CommonName == name);
        }

        public float CalculatePlatypusCoolness(Platypus platypus, bool considerBillLength = true)
        {
            _platypuses.Add(platypus);
            var initialValue = platypus.BillPowerCoeffecient & platypus.BillDisplacement;
            if (considerBillLength)
            {
                initialValue *= platypus.BillLength;
            }

            return initialValue;
        }

        public List<StringSegment> GetScienceyWords(int limit)
        {
            return _scienceyWords;
        }
    }
}
