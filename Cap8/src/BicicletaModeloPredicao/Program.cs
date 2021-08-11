using System;
using System.Reflection;
using System.IO;
using Microsoft.ML.Data;
using Microsoft.ML;
using BicicletaModeloPredicao.Classes;

namespace BicicletaModeloPredicao
{
    class Program
    {
        private static readonly string _modelDirectoryPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modelos");

        private static MLContext _mlContext;

        static void Main(string[] args)
        {
            var exemplos = BikeHorasInstanciaExemplo.GetExemplos();

            Console.WriteLine($"Total de exemplos: {exemplos.Count}");

            _mlContext = new MLContext(seed: 34);
        }
    }
}
