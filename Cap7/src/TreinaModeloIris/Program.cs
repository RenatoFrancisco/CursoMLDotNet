using System;
using System.IO;
using System.Reflection;
using Microsoft.ML;

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

            Console.WriteLine("Finalizando programa");
            Console.ReadKey();
        }
    }
}
