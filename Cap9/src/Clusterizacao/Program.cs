using System;
using System.IO;
using System.Reflection;
using Microsoft.ML;
using Microsoft.ML.Data;
using Clusterizacao.Classes;

namespace Clusterizacao
{
    class Program
    {
        private static string _dadosPath = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dados", "iris-full.txt");

        private static MLContext _mlContext;

        static void Main(string[] args)
        {
            if (!File.Exists(_dadosPath))
            {
                Console.WriteLine($"Dados não encontrados {_dadosPath}");
                Console.ReadKey();
                return;
            }

            _mlContext = new MLContext(seed: 1234);

            var colunas = new[]
            {
                new TextLoader.Column(DefaultColumnNames.Label, DataKind.Single, 0),
                new TextLoader.Column(nameof(IrisData.SepalLength), DataKind.Single, 1),
                new TextLoader.Column(nameof(IrisData.SepalWidth), DataKind.Single, 2),
                new TextLoader.Column(nameof(IrisData.PetalLength), DataKind.Single, 3),
                new TextLoader.Column(nameof(IrisData.PetalWidth), DataKind.Single, 4),
            };

            // Carga do arquivo
            var dataViewDados = 
                _mlContext.Data.LoadFromTextFile(_dadosPath, columns: colunas, separatorChar: '\t', hasHeader: true);

            // Divisão do dados
            var dadosTreinoTeste = _mlContext.Clustering.TrainTestSplit(dataViewDados, testFraction: 0.25);

            // Concatenação das colunas
            var pipelineProcessamento = 
                _mlContext.Transforms.Concatenate(
                    DefaultColumnNames.Features,
                    nameof(IrisData.SepalLength),
                    nameof(IrisData.SepalWidth),
                    nameof(IrisData.PetalLength),
                    nameof(IrisData.PetalWidth));

            ITransformer trainerModel3Clusters = default;

            for (var i = 2; i <= 10; i++)
            {
                // Treinamento do modelo
                var trainer = 
                    _mlContext.Clustering.Trainers.KMeans(featureColumnName: DefaultColumnNames.Features, clustersCount: i);

                var pipelineTreinamento = pipelineProcessamento.Append(trainer);

                var trainedModel =  pipelineTreinamento.Fit(dadosTreinoTeste.TrainSet);

                if (i == 3) trainerModel3Clusters = trainedModel;

                // Avaliação do modelo
                var predictions = trainedModel.Transform(dadosTreinoTeste.TrainSet);
                var metrics = 
                    _mlContext.Clustering.Evaluate(predictions, score: DefaultColumnNames.Score, features: DefaultColumnNames.Features);

                Console.WriteLine($"{metrics.AvgMinScore} para {i} clusters");
            }

            var predEngine = trainerModel3Clusters.CreatePredictionEngine<IrisData, IrisPrediction>(_mlContext);

            // Iris setosa:     5.0 3.6 1.4 0.2
            // Iris virginica:  5.7	2.6	3.5	1.0
            // Iris versicolor: 6.7	3.0	5.2	2.3

            var exemplo1 = new IrisData()
            {
                // Iris-setosa
                SepalLength = 5.0f,
                SepalWidth = 3.6f,
                PetalLength = 1.4f,
                PetalWidth = 0.2f,
            };

            var exemplo2 = new IrisData()
            {
                // Iris-virginica
                SepalLength = 5.7f,
                SepalWidth = 2.6f,
                PetalLength = 3.5f,
                PetalWidth = 1.0f,
            };

            var exemplo3 = new IrisData()
            {
                // Iris-versicolor
                SepalLength = 6.7f,
                SepalWidth = 3.0f,
                PetalLength = 5.2f,
                PetalWidth = 2.3f,
            };

            var resultado1 = predEngine.Predict(exemplo1);
            Console.WriteLine($"{resultado1.ClusterId} para uma instância de iris-setosa");
            Console.WriteLine($"{predEngine.Predict(exemplo2).ClusterId} para uma instância iris-virginica");
            Console.WriteLine($"{predEngine.Predict(exemplo3).ClusterId} para uma instância iris-versicolor");
        }
    }
}
