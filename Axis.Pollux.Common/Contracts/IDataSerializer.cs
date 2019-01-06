using Axis.Luna.Operation;
using System;

namespace Axis.Pollux.Common.Contracts
{
    public interface IDataSerializer
    {
        Operation<string> SerializeData<TData>(TData data);
        Operation<string> SerializeData(object data);
        Operation<string> SerializeData(Type objectType, object data);

        Operation<TData> Deserialize<TData>(string serialFormat);
        Operation<object> Deserialize(Type dataType, string serialFormat);
    }
}
