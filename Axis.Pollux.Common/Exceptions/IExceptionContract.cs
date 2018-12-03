namespace Axis.Pollux.Common.Exceptions
{
    public interface IExceptionContract
    {
        string Code { get; }
        string Message { get; }
        object Info { get; }
    }
}
