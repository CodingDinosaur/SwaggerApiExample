using FlowFitExample.Models;
using System.Collections.Generic;

namespace FlowFitExample.Managers
{
    public interface IScienceManager
    {
        DinosaurGenome RegisterNewDinosaurGenome(string name, byte[] geneticData);
        DinosaurGenome GetDinosaurGenomeByName(string name);
        float CalculatePlatypusCoolness(Platypus platypus, bool considerBillLength = true);
        List<StringSegment> GetScienceyWords(int limit);
    }
}
