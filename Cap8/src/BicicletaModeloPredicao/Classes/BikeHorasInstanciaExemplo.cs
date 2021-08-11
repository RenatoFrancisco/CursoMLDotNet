using System.Collections.Generic;
using BicicletaModeloTreino.Classes;

namespace BicicletaModeloPredicao.Classes
{
    public class BikeHorasInstanciaExemplo
    {
        static List<BikeHoraInstancia> _exemplos =
            new List<BikeHoraInstancia>()
            {
                new BikeHoraInstancia
                    {
                        Season = 3,
                        Year = 1,
                        Month = 8,
                        Hour = 18,
                        Holiday = 0,
                        Weekday = 4,
                        WorkingDay = 1,
                        Weather = 2,
                        Temperature = 0.82f,
                        NormalizedTemperature = 0.7576f,
                        Humidity = 0.46f,
                        Windspeed = 0.0896f,
                        Count = 811
                    },
                new BikeHoraInstancia
                    {
                        Season = 4,
                        Year = 1,
                        Month = 11,
                        Hour = 23,
                        Holiday = 0,
                        Weekday = 5,
                        WorkingDay = 1,
                        Weather = 1,
                        Temperature = 0.32f,
                        NormalizedTemperature = 0.3333f,
                        Humidity = 0.81f,
                        Windspeed = 0.0896f,
                        Count = 162
                    },
                new BikeHoraInstancia
                    {
                        Season = 3,
                        Year = 1,
                        Month = 9,
                        Hour = 18,
                        Holiday = 0,
                        Weekday = 2,
                        WorkingDay = 1,
                        Weather = 1,
                        Temperature = 0.64f,
                        NormalizedTemperature = 0.6212f,
                        Humidity = 0.36f,
                        Windspeed = 0.1642f,
                        Count = 877
                    }
            };

        public static List<BikeHoraInstancia> GetExemplos() => _exemplos;
    }
}