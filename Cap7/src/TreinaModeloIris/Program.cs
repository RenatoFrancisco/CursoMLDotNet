using System;
using System.IO;
using System.Reflection;
using Microsoft.ML;
using Microsoft.ML.Data;
using TreinaModeloIris.Classses;

namespace TreinaModeloIris
{
    class Program
    {
        private static readonly string _dataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dados", "iris-full.txt");

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando programa");

            // Primeira etapa: Carregar os dados
            if (!File.Exists(_dataPath))
            {
                Console.WriteLine("Dataset não encontrado");
                Console.ReadKey();
                return;
            }

            var mlContext = new MLContext(seed: 1234);
            var dataView = mlContext.Data.LoadFromTextFile<IrisData>(_dataPath, hasHeader: true);
            var spitData = mlContext.MulticlassClassification.TrainTestSplit(dataView, testFraction: 0.25);

            // Segunda etapa: transformação dos dados
            var dataProcessPipeline = mlContext.Transforms.Concatenate(
                DefaultColumnNames.Features,
                nameof(IrisData.SepalWidth),
                nameof(IrisData.SepalLength),
                nameof(IrisData.PetalWidth),
                nameof(IrisData.PetalLength)
            ).AppendCacheCheckpoint(mlContext);

            Console.WriteLine("Finalizando programa");
            Console.ReadKey();
        }
    }
}
