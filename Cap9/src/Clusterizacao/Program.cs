using System;
using System.IO;
using System.Reflection;
using Microsoft.ML;
using Microsoft.ML.Data;


namespace Clusterizacao
{
    class Program
    {
        private static string _dadosPath = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dados", "iris-full");

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
        }
    }
}
