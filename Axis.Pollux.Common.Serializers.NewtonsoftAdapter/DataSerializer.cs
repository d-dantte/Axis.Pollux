using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Contracts;
using Newtonsoft.Json;

namespace Axis.Pollux.Common.Serializers.NewtonsoftAdapter
{
    public class DataSerializer: IDataSerializer
    {
        private readonly SerializerConfigProvider _configProvider;

        public DataSerializer(SerializerConfigProvider configProvider)
        {
            _configProvider = configProvider?.Clone() ?? new SerializerConfigProvider();
        }

        public Operation<string> SerializeData<TData>(TData data)
        => Operation.Try(() =>
        {
            var settings = _configProvider.ConfigurationFor<TData>();
            return settings == null 
                ? JsonConvert.SerializeObject(data) 
                : JsonConvert.SerializeObject(data, settings);
        });

        public Operation<string> SerializeData(object data) => SerializeData(data?.GetType(), data);

        public Operation<string> SerializeData(Type objectType, object data)
        => Operation.Try(() =>
        {
            if (data == null)
                return JsonConvert.SerializeObject(null);

            var settings = _configProvider.ConfigurationFor(objectType);
            return settings == null
                ? JsonConvert.SerializeObject(data)
                : JsonConvert.SerializeObject(data, settings);
        });

        public Operation<TData> Deserialize<TData>(string serialFormat)
        => Operation.Try(() =>
        {
            var settings = _configProvider.ConfigurationFor<TData>();
            return settings == null
                ? JsonConvert.DeserializeObject<TData>(serialFormat)
                : JsonConvert.DeserializeObject<TData>(serialFormat, settings);
        });

        public Operation<object> Deserialize(Type dataType, string serialFormat)
        => Operation.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(serialFormat))
                return null;

            var settings = _configProvider.ConfigurationFor(dataType);
            return settings == null
                ? JsonConvert.DeserializeObject(serialFormat, dataType)
                : JsonConvert.DeserializeObject(serialFormat, dataType, settings);
        });
    }
}