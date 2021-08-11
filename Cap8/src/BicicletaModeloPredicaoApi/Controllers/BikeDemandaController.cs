using Microsoft.AspNetCore.Mvc;
using BicicletaModeloTreino.Classes;
using Microsoft.ML.Data;
using Microsoft.ML;
using System.IO;
using System.Reflection;

namespace BicicletaModeloPredicaoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BikeDemandaController : ControllerBase
    {
        private static readonly string _modelDirectoryPath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modelos", "FastTree.tar");

         private static MLContext _mlContext;

         private static ITransformer _trainedModel;

         private static PredictionEngine<BikeHoraInstancia, BikeHoraPredicao> _predEngine;

         public BikeDemandaController()
         {
             _mlContext = new MLContext();

             using var fs = new FileStream(
                 _modelDirectoryPath, 
                 FileMode.Open, 
                 FileAccess.Read, 
                 FileShare.Read);

            _trainedModel = _mlContext.Model.Load(fs);
            _predEngine = _trainedModel.CreatePredictionEngine<BikeHoraInstancia, BikeHoraPredicao>(_mlContext);
         }

        [HttpGet]
        public float Get([FromQuery]BikeHoraInstancia instancia)
        {
            var prediction = _predEngine.Predict(instancia);
            return prediction.PredictedCount;
        }
    }
}