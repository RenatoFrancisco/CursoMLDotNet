using System;
using System.Reflection;
using System.IO;
using Microsoft.ML.Data;
using Microsoft.ML;
using BicicletaModeloPredicao.Classes;
using BicicletaModeloTreino.Classes;

namespace BicicletaModeloPredicao
{
    class Program
    {
        private static readonly string _modelDirectoryPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modelos");

        private static MLContext _mlContext;

        private static readonly string[] _nomesAlgoritmos = new string[] { "FastTree", "SDCA" };

        static void Main(string[] args)
        {
            var exemplos = BikeHorasInstanciaExemplo.GetExemplos();

            Console.WriteLine($"Total de exemplos: {exemplos.Count}");

            foreach (var algoritmo in _nomesAlgoritmos)
            {
                var modelFilePath = Path.Combine(_modelDirectoryPath, $"{algoritmo}.tar");
                if (!File.Exists(modelFilePath))
                {
                    Console.WriteLine($"Modelo não encontrado: {modelFilePath}");
                    Console.ReadKey();
                    return;
                }

                using var fs = new FileStream(
                    modelFilePath, 
                    FileMode.Open, 
                    FileAccess.Read, 
                    FileShare.Read);

                _mlContext = new MLContext();
                var trainedModel = _mlContext.Model.Load(fs);
                var predictionEngine = trainedModel.CreatePredictionEngine<BikeHoraInstancia, BikeHoraPredicao>(_mlContext);

                var i = 0;
                foreach (var exemplo in exemplos)
                {
                    var prediction = predictionEngine.Predict(exemplo);
                    Console.WriteLine($"{algoritmo} exemplo {i++} Demanda real {exemplo.Count} Demanda prevista {prediction.PredictedCount}");
                }

                Console.WriteLine();
            }

        }
    }
}
