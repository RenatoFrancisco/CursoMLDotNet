using Microsoft.ML.Data;

namespace SistemasDeRecomendacao.Classes
{
    public class FilmeAvaliacao
    {
        [LoadColumn(0)]
        public float userId;

        [LoadColumn(1)]
        public float movieId;

        [LoadColumn(2)]
        public float Label;
    }
}
