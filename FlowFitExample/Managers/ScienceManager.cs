using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FlowFitExample.Models;
using Microsoft.Extensions.Logging;

namespace FlowFitExample.Managers
{
    public class ScienceManager : IScienceManager
    {
        private readonly List<(string Hash, DinosaurGenome Genome)> _dinosaurs = new List<(string, DinosaurGenome)>();
        private readonly List<Platypus> _platypuses = new List<Platypus>();
        private readonly ILogger<ScienceManager> _log;

        private readonly List<StringSegment> _scienceyWords = new List<StringSegment>
        {
            new StringSegment("enUS", "Tyrannosaurus Rex"),
            new StringSegment("frFR", "Le Tyrannosaurus Rex"),
            new StringSegment("esMX", "El Tyrannosaurus Rex"),
            new StringSegment("itIT", "Eugotta Runnawayo"),
        };

        public ScienceManager(ILogger<ScienceManager> log)
        {
            _log = log;
        }

        public DinosaurGenome RegisterNewDinosaurGenome(string name, byte[] geneticData)
        {
            var newGenome = new DinosaurGenome(name, name + " (but fancy)", geneticData);
            var hash = ComputeDinosaurHash(newGenome);
            _dinosaurs.Add((hash, newGenome));
            _log.LogInformation($"Added new Dinosaur Genome: {newGenome.CommonName}, {newGenome.ScienceyName}, {hash}");

            return newGenome;
        }

        public DinosaurGenome GetDinosaurGenomeByName(string name)
        {
            return _dinosaurs.FirstOrDefault(d => d.Genome.CommonName == name).Genome;
        }

        public void DeactivateDinosaurGenome(string hash)
        {
            var dinoToRemove = _dinosaurs.SingleOrDefault(d => d.Hash == hash);
            if (dinoToRemove == default) { throw new InvalidOperationException($"Unable to locate single Dinosaur with hash {hash}"); }

            _dinosaurs.RemoveAll(d => d.Hash == hash);

            _log.LogInformation($"Removed Dinosaur Genome: {dinoToRemove.Genome.CommonName}, {dinoToRemove.Hash}");
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

        private static string ComputeDinosaurHash(DinosaurGenome genome)
        {
            using var ms = new MemoryStream();
            using var md5 = MD5.Create();

            ms.Write(genome.TopSecretGeneticData);
            ms.Write(Encoding.UTF8.GetBytes(genome.ScienceyName)); // Salt
            var hash = md5.ComputeHash(ms);
            return Convert.ToBase64String(hash);
        }
    }
}
