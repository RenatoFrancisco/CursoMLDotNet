using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.ML;
using Microsoft.ML.Data;
using TreinaModeloIris.Classes;

namespace TreinaModeloIris
{
    class Program
    {
        private static readonly string _dataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dados", "iris-full.txt");
        private static readonly string _modelPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modelo.tar");

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
            var splitData = mlContext.MulticlassClassification.TrainTestSplit(dataView, testFraction: 0.25);

            // Segunda etapa: transformação dos dados
            var dataProcessPipeline = mlContext.Transforms.Concatenate(
                DefaultColumnNames.Features,
                nameof(IrisData.SepalWidth),
                nameof(IrisData.SepalLength),
                nameof(IrisData.PetalWidth),
                nameof(IrisData.PetalLength)
            )
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: DefaultColumnNames.Label, inputColumnName: nameof(IrisData.Label)))
            .AppendCacheCheckpoint(mlContext);

            // Terceira etapa: Treinamento de um modelo de Machine Learning
            var trainer = mlContext.MulticlassClassification
                .Trainers
                .LogisticRegression(labelColumnName: "Label", featureColumnName: DefaultColumnNames.Features);

            var watch = Stopwatch.StartNew();

            var trainingPipeline = dataProcessPipeline.Append(trainer);
            var trainedModel = trainingPipeline.Fit(splitData.TrainSet);

            watch.Stop();

            Console.WriteLine($"{watch.ElapsedMilliseconds / 1000} segundos");

            // Quarta etapa: Avaliação do Modelo
            var predictions = trainedModel.Transform(splitData.TestSet);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictions, DefaultColumnNames.Label, DefaultColumnNames.Score);

            Console.WriteLine($"Acurácia: {metrics.AccuracyMacro}"); // 0 a 1, quanto maior -> melhor
            Console.WriteLine($"LogLoss: {metrics.LogLoss}"); // 0 a 1, quanto menor -> melhor
            Console.WriteLine($"LogLoss para classe 1: {metrics.PerClassLogLoss[0]}");
            Console.WriteLine($"LogLoss para classe 2: {metrics.PerClassLogLoss[1]}");
            Console.WriteLine($"LogLoss para classe 3: {metrics.PerClassLogLoss[2]}");

            // Quinta etapa: Serializando o modelo (salvando o modelo em um arquivo)
            using var fs = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write);
            mlContext.Model.Save(trainedModel, fs);
            Console.WriteLine($"Modelo salvo em: {_modelPath}");

            Console.WriteLine("Finalizando programa");
        }
    }
}
