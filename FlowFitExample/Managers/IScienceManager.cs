using System;
using FlowFitExample.Models;
using System.Collections.Generic;

namespace FlowFitExample.Managers
{
    public interface IScienceManager
    {
        DinosaurGenome RegisterNewDinosaurGenome(string name, Span<byte> geneticData);
        DinosaurGenome GetDinosaurGenomeByName(string name);
        void DeactivateDinosaurGenome(string hash);
        float CalculatePlatypusCoolness(Platypus platypus, bool considerBillLength = true);
        List<StringSegment> GetScienceyWords(int limit);
    }
}
