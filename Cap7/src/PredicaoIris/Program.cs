using System;
using System.IO;
using Microsoft.ML;
using System.Reflection;

namespace PredicaoIris
{
    class Program
    {
        private static readonly string _modelPath = 
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modelos", "Modelo.tar");

        private static readonly MLContext _mlContext = new MLContext();

        private static ITransformer _trainedModel;

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando programa");
            Load();
            Console.WriteLine("Finalizando programa");
        }

        private static void Load()
        {
            using var fs = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _trainedModel = _mlContext.Model.Load(fs);
        }
    }
}
