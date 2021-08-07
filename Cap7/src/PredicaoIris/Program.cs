using System;
using System.IO;
using Microsoft.ML;
using System.Reflection;
using TreinaModeloIris.Classes;

namespace PredicaoIris
{
    class Program
    {
        private static readonly string _modelPath = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modelos", "Modelo.tar");

        private static readonly MLContext _mlContext = new MLContext();

        private static ITransformer _trainedModel;

        private static PredictionEngine<IrisData, IrisPrediction> _predictionEngine;

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando programa");

            Load();

            var sepalLength = 4.7f;
            var sepalWidth = 3.2f;
            var petalLength = 1.3f;
            var petalWidth = 0.2f;

            var amostra = new IrisData
            {
                SepalLength = sepalLength,
                SepalWidth = sepalWidth,
                PetalLength = petalLength,
                PetalWidth = petalWidth
            };

            var result = _predictionEngine.Predict(amostra);

            Console.WriteLine($"Prob. Classe 0: {result.Score[0]}");
            Console.WriteLine($"Prob. Classe 1: {result.Score[1]}");
            Console.WriteLine($"Prob. Classe 2: {result.Score[2]}");

            Console.WriteLine("Finalizando programa");
        }

        private static void Load()
        {
            using var fs = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _trainedModel = _mlContext.Model.Load(fs);
            _predictionEngine = _trainedModel.CreatePredictionEngine<IrisData, IrisPrediction>(_mlContext);
        }
    }
}
