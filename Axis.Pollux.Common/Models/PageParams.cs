using Axis.Luna;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Pollux.Common.Models
{
    public class PageParams
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool IncludeCount { get; set; }

        public SequencePage<Model> Paginate<Model, Entity>(IOrderedQueryable<Entity> query, Func<IQueryable<Entity>, IEnumerable<Model>> transformer)
        {
            var psize = Math.Abs(PageSize);
            var pindex = Math.Abs(PageIndex);

            var _query = query.Skip(psize * pindex);

            if (psize >= 0) _query = _query.Take(psize);

            var d = transformer
                .Invoke(_query)
                .ToArray();

            var count = IncludeCount ? query.LongCount() : d.Length;
            return new SequencePage<Model>(d, count, psize, pindex);
        }


        public static PageParams EntireSequence(bool includeCount = true)
        => new PageParams
        {
            PageSize = -1, //meaning assume the size of the array returned,
            PageIndex = 0,
            IncludeCount = includeCount
        };
    }
}
