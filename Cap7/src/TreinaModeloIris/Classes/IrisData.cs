using Microsoft.ML.Data;
using System;

namespace TreinaModeloIris.Classses
{
    public class IrisData
    {
        [LoadColumn(0)]
        public float Label { get; set; }

        [LoadColumn(1)]
        public float SepalLenght { get; set; }

        [LoadColumn(2)]
        public float SepalWidth { get; set; }

        [LoadColumn(3)]
        public float PetalLength { get; set; }

        [LoadColumn(4)]
        public float PetalWidth { get; set; }
    }
}