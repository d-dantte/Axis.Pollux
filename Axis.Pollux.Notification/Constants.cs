using Newtonsoft.Json;

namespace Axis.Pollux.Notification
{
    public static class Constants
    {
        public static JsonSerializerSettings NotificationDataSerializationSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            FloatFormatHandling = FloatFormatHandling.DefaultValue,
            StringEscapeHandling = StringEscapeHandling.Default,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
    }
}
