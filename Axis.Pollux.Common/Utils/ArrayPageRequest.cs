namespace Axis.Pollux.Common.Utils
{
    public class ArrayPageRequest
    {
        public uint? PageIndex { get; set; }
        public uint? PageSize { get; set; }
        public bool IncludeTotalLength { get; set; } = false;

        public ArrayPageRequest Normalize(uint maxPageSize = 100) => new ArrayPageRequest
        {
            IncludeTotalLength = IncludeTotalLength,
            PageIndex = PageIndex ?? 0,
            PageSize = (PageSize == null || PageSize > maxPageSize) ? maxPageSize : PageSize
        };

        public static ArrayPageRequest CreateNormalizedRequest() => new ArrayPageRequest().Normalize();
    }
}
