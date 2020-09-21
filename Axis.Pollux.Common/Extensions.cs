using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;

namespace Axis.Pollux.Common
{
    public static class Extensions
    {
        public static void Throw(this OperationError operationError) => throw new OperationException(operationError);

        public static Enm ParseEnum<Enm>(this string @string)
        where Enm: struct
        {
            if (!Enum.TryParse<Enm>(@string, out var enm))
                throw new Exception("Invalid Enum Conversion");

            else
                return enm;
        }

        public static bool TryParseEnum<Enm>(this string @string, out Enm @enum)
        where Enm : struct => Enum.TryParse(@string, out @enum);

        public static bool IsNull(this object value) => value == null;

        public static bool IsNotNull(this object value) => value != null;

        public static async Task<Data[]> GetAll<Data>(this ArrayPageRequest request, Func<ArrayPageRequest, Task<ArrayPage<Data>>> query)
        {
            request = request?.Normalize() ?? ArrayPageRequest.CreateNormalizedRequest();
            ArrayPage<Data> dataPage = null;
            var dataList = new List<Data>();

            while((dataPage = await query.Invoke(request)).Page.Length != 0)
            {
                dataList.AddRange(dataPage.Page);
                request = request.NextPage();
            }

            return dataList.ToArray();
        }

        public static ArrayPageRequest NextPage(this ArrayPageRequest request)
        => new ArrayPageRequest
        {
            PageSize = request.PageSize,
            PageIndex = request.PageIndex + 1
        };

        public static ArrayPageRequest PreviousPage(this ArrayPageRequest request)
        => new ArrayPageRequest
        {
            PageSize = request.PageSize,
            PageIndex = request.PageIndex > 0
                ? request.PageIndex - 1
                : 0
        };
    }
}
