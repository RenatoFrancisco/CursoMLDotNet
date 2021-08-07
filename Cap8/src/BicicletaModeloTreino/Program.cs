using System;
using System.IO;
using System.Reflection;
using Microsoft.ML;
using BicicletaDemandaTreino.Classes;
using Microsoft.ML.Data;

namespace BicicletaModeloTreino
{
    class Program
    {
        private static readonly string _dadosTreinoPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dados", "demanda_bike_hora_treino.csv");

        private static readonly string _dadosTestePath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dados", "demanda_bike_hora_teste.csv");

        private static MLContext _mlContext;

        static void Main(string[] args)
        {
            if (!File.Exists(_dadosTreinoPath) || !File.Exists(_dadosTreinoPath))
            {
                Console.WriteLine("Datasets não encontrados!");
                return;
            }

            _mlContext = new MLContext(seed: 34);
            var treinoDataView = _mlContext.Data.LoadFromTextFile<BikeHoraInstancia>(_dadosTreinoPath, separatorChar: ',', hasHeader: true);
            var testeDataView = _mlContext.Data.LoadFromTextFile<BikeHoraInstancia>(_dadosTestePath, separatorChar: ',', hasHeader: true);

            var pipelineProcessamento = _mlContext.Transforms.Concatenate(
                DefaultColumnNames.Features,
                nameof(BikeHoraInstancia.Season),
                nameof(BikeHoraInstancia.Year),
                nameof(BikeHoraInstancia.Month),
                nameof(BikeHoraInstancia.Hour),
                nameof(BikeHoraInstancia.Holiday),
                nameof(BikeHoraInstancia.Weekday),
                nameof(BikeHoraInstancia.WorkingDay),
                nameof(BikeHoraInstancia.Weather),
                nameof(BikeHoraInstancia.Temperature),
                nameof(BikeHoraInstancia.NormalizedTemperature),
                nameof(BikeHoraInstancia.Humidity),
                nameof(BikeHoraInstancia.Windspeed)
            ).AppendCacheCheckpoint(_mlContext);

            (string nome, IEstimator<ITransformer> algoritmo)[] algoritmosRegressao =
            {
                ("FastTree", _mlContext.Regression.Trainers.FastTree()),
                ("SDCA", _mlContext.Regression.Trainers.StochasticDualCoordinateAscent() )
            };

            foreach (var item in algoritmosRegressao)
            {
                var pipelineTreinamento = pipelineProcessamento.Append(item.algoritmo);
                var modeloTreinado = pipelineProcessamento.Fit(treinoDataView);
            }

            Console.WriteLine("Programa finalizado");
        }
    }
}
