namespace Axis.Pollux.Common.Utils
{
    public class ArrayPageRequest
    {
        public uint? PageIndex { get; set; }
        public uint? PageSize { get; set; }

        public virtual ArrayPageRequest Normalize(uint maxPageSize = 100) => new ArrayPageRequest
        {
            PageIndex = PageIndex ?? 0,
            PageSize = (PageSize==null|| PageSize > maxPageSize)? maxPageSize : PageSize
        };
    }
}
