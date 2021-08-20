using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.IO;
using System.Linq;
using System.Reflection;
using SistemasDeRecomendacao.Classes;

namespace SistemasDeRecomendacao
{
    class Program
    {
        private static readonly string _dadosTreinoPath = 
             Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Datasets", "recommendation-ratings-train.csv");

        private static readonly string _dadosTestePath = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Datasets", "recommendation-ratings-test.csv");

        private static readonly string _dadosFilmesPath = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Datasets", "recommendation-movies.csv");

        private static MLContext _mlContext;

        static void Main(string[] args)
        {
            if (!File.Exists(_dadosFilmesPath) || !File.Exists(_dadosTreinoPath) || !File.Exists(_dadosTestePath))
            {
                Console.WriteLine("Dados não encontrados");
                Console.ReadKey();
                return;
            }

            _mlContext = new MLContext();

            var dataViewTreino =
                 _mlContext.Data.LoadFromTextFile<FilmeAvaliacao>(_dadosTreinoPath, hasHeader: true, separatorChar: ',');

        }
    }
}
