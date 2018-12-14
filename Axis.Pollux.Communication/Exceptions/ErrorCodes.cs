namespace Axis.Pollux.Communication.Exceptions
{
    public static class ErrorCodes
    {
        /// <summary>
        /// When an the ISystemChannelSourceAddressProvider.GetChannelSourceAddress returns null
        /// </summary>
        public static readonly string InvalidChannelSourceAddress = "Pollux.Communication.Error.0";

        /// <summary>
        /// When an appropriate CommChannel is not found to send a message through
        /// </summary>
        public static readonly string NoSuitableChannelFound = "Pollux.Communication.Error.1";
    }
}
