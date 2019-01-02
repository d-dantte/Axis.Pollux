using System;
using System.Collections.Generic;
using Axis.Luna.Extensions;
using Newtonsoft.Json;

namespace Axis.Pollux.Common.Serializers.NewtonsoftAdapter
{
    public class SerializerConfigProvider
    {
        private readonly Dictionary<Type, JsonSerializerSettings> _settingsTable = new Dictionary<Type, JsonSerializerSettings>();
        private readonly JsonSerializerSettings _defaultSettings;

        public SerializerConfigProvider(JsonSerializerSettings defaultSettings = null)
        {
            _defaultSettings = defaultSettings;
        }

        public JsonSerializerSettings ConfigurationFor<TData>()
        => _settingsTable.TryGetValue(typeof(TData), out var value) ? value : _defaultSettings;

        public JsonSerializerSettings ConfigurationFor(Type dataType)
        => _settingsTable.TryGetValue(dataType, out var value) ? value : _defaultSettings;


        public SerializerConfigProvider Configure<TData>(JsonSerializerSettings settings)
        {
            if(settings == null)
                throw new ArgumentNullException(nameof(settings));

            _settingsTable.Add(typeof(TData), settings);
            return this;
        }

        public SerializerConfigProvider Configure(Type type, JsonSerializerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            _settingsTable.Add(type, settings);
            return this;
        }

        public SerializerConfigProvider Clone()
        {
            var provider = new SerializerConfigProvider(_defaultSettings);
            _settingsTable.ForAll(kvp => provider.Configure(kvp.Key, kvp.Value));

            return provider;
        }
    }
}
