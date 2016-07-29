using Axis.Luna.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Axis.Pollux.ABAC.Utils
{
    public static class JsonSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Converters = new StringEnumConverter().Enumerate() //<-- add other converters here
                                                  .Cast<JsonConverter>()
                                                  .ToList()
        };
    }
}
