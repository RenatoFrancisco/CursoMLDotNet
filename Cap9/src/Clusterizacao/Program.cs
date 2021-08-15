﻿using System;
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

            var trainer = 
                _mlContext.Clustering.Trainers.KMeans(featureColumnName: DefaultColumnNames.Features, clustersCount: 3);

            var pipelineTreinamento = pipelineProcessamento.Append(trainer);

            var trainedModel =  pipelineTreinamento.Fit(dadosTreinoTeste.TrainSet);
        }
    }
}
