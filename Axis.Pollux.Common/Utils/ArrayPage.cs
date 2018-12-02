using System;
using System.Collections.Generic;
using System.Text;

namespace Axis.Pollux.Common.Utils
{
    public class ArrayPage<Data>
    {
        public Data[] Page { get; set; }
        public uint PageIndex { get; set; }
        public uint PageSize { get; set; }
        public long TotalLength { get; set; }
    }
}
