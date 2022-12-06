using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;

namespace API.Infrastructure.Logger
{
    public class LogDestructureModel : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            result = new ScalarValue(JsonConvert.SerializeObject(value));
            return true;
        }
    }
}
