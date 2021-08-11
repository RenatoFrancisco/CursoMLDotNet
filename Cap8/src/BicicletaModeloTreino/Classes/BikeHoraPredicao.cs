using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BicicletaModeloTreino.Classes
{
    public class BikeHoraPredicao
    {
        [ColumnName("Score")]
        public float PredictedCount;
    }
}