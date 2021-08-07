using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bicicleta_Demanda_Treino.Classes
{
    public class BikeHoraPredicao
    {
        [ColumnName("Score")]
        public float PredictedCount;
    }
}