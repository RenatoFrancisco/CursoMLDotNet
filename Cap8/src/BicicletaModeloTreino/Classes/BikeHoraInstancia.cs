using Microsoft.ML.Data;

namespace BicicletaModeloTreino.Classes
{
    public class BikeHoraInstancia
    {
        /* 
         * Observe que não estamos utilizando algumas colunas, como a 0 (índice) e a 1 (data)
         * e as columnas 14 e 15 (quantidade de usuários casuais e registrados)
         */
        
        [LoadColumn(2)]
        public float Season { get; set; }

        [LoadColumn(3)]
        public float Year { get; set; }

        [LoadColumn(4)]
        public float Month { get; set; }

        [LoadColumn(5)]
        public float Hour { get; set; }

        [LoadColumn(6)]
        public float Holiday { get; set; }

        [LoadColumn(7)]
        public float Weekday { get; set; }

        [LoadColumn(8)]
        public float WorkingDay { get; set; }

        [LoadColumn(9)]
        public float Weather { get; set; }

        [LoadColumn(10)]
        public float Temperature { get; set; }

        [LoadColumn(11)]
        public float NormalizedTemperature { get; set; }

        [LoadColumn(12)]
        public float Humidity { get; set; }

        [LoadColumn(13)]
        public float Windspeed { get; set; }

        [LoadColumn(16)]
        [ColumnName("Label")] // Importante anotar como Label, afinal será o valor que desejamos prever
        public float Count { get; set; }  
    }
}
