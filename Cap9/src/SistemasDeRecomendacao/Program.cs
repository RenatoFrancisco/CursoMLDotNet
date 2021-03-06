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

            // Carga dos Dados
            var dataViewTreino =
                 _mlContext.Data.LoadFromTextFile<FilmeAvaliacao>(_dadosTreinoPath, hasHeader: true, separatorChar: ',');

            // Transformando os dados para ser entregue ao algoritmo de fatorização de matrizes
            var pipelineProcessamento = 
                _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userId", inputColumnName: nameof(FilmeAvaliacao.userId))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieId", inputColumnName: nameof(FilmeAvaliacao.movieId)));

            var options = new MatrixFactorizationTrainer.Options();
            options.MatrixRowIndexColumnName = "userId";
            options.MatrixColumnIndexColumnName = "movieId";
            options.LabelColumnName = DefaultColumnNames.Label;
            options.NumberOfIterations = 100;

            var pipelineTreinamento = 
                pipelineProcessamento.Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));

            var model = pipelineTreinamento.Fit(dataViewTreino);

            // Avaliação de Modelo
            var dataViewTeste = 
                _mlContext.Data.LoadFromTextFile<FilmeAvaliacao>(_dadosTestePath, hasHeader: true, separatorChar: ',');

            var predicoes = model.Transform(dataViewTeste);
            var metricas = _mlContext.Regression.Evaluate(predicoes, label: DefaultColumnNames.Label, score: DefaultColumnNames.Score);
            Console.WriteLine($"RMS do Modelo: {metricas.Rms}");

            // Obtendo predições
            var predEngine = model.CreatePredictionEngine<FilmeAvaliacao, FilmeAvaliacaoPredicao>(_mlContext);

            var linhas = File.ReadAllLines(_dadosTestePath);
            foreach (var linha in linhas.Skip(1))
            {
                var campos = linha.Split(',');

                var amostra = new FilmeAvaliacao
                {
                    userId = float.Parse(campos[0]),
                    movieId = float.Parse(campos[1]),
                    Label = float.Parse(campos[2].Replace('.', ','))
                };

                var predicao = predEngine.Predict(amostra);

                var resultadoFinal = predicao.Score;

                if (resultadoFinal < 0)
                    resultadoFinal = 0;
                else if (resultadoFinal > 5)
                    resultadoFinal = 5;
                else resultadoFinal = (float) Math.Round(resultadoFinal, 1);

                var resultado = $"Usuário {amostra.userId} avaliou filme {amostra.movieId} como {amostra.Label} e o modelo previou como {resultadoFinal}";
                Console.WriteLine(resultado);
            }
        }
    }
}
