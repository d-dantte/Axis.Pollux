namespace Axis.Pollux.UserCommon.Models
{
    public class PageParams
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool IncludeCount { get; set; }


        public static PageParams EntireSequence(bool includeCount = true)
        => new PageParams
        {
            PageSize = 0, //meaning assume the size of the array returned,
            PageIndex = 0,
            IncludeCount = includeCount
        };
    }
}
