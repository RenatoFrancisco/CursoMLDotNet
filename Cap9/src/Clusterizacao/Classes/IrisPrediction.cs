using Microsoft.ML.Data;

namespace Clusterizacao.Classes
{
    public class IrisPrediction
    {
        [ColumnName("PredictedLabel")]
        public int ClusterId { get; set; }

        [ColumnName("Score")]
        public float[] Distance { get; set; }
    }
}