using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace PasswordTool.Web.Results
{
    public class JsonResult<TModel> : ActionResult
    {
        private readonly TModel _model;

        public JsonResult(TModel model)
        {
            _model = model;
        }

        public TModel Model
        {
            get { return _model; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var payload = JsonConvert.SerializeObject(_model,
                                                      new JsonSerializerSettings
                                                      {
                                                          ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                          Converters = new List<JsonConverter> { new StringEnumConverter() }
                                                      });
            context.HttpContext.Response.Write(payload);
        }
    }
}