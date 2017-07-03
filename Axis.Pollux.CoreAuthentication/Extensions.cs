using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Pollux.CoreAuthentication
{
    public static class Extensions
    {
        public enum FoldType
        {
            All,
            Any,
            None
        }

        public static IOperation Then(this IEnumerable<IOperation> operations, Action action, FoldType foldType = FoldType.All)
        => LazyOp.Try(() =>
        {
            var ops = operations.UsingEach(_op =>
            {
                try { _op.Resolve(); }
                catch { }
            })
            .ToArray();

            switch (foldType)
            {
                case FoldType.All:
                    if (!ops.All(_op => _op.Succeeded == true))
                        throw new AggregateException(ops.Where(_op => _op.Succeeded == false).Select(_op => _op.GetException()).ToArray());
                    else break;

                case FoldType.None:
                    if (ops.Any(_op => _op.Succeeded == true))
                        throw new Exception("Some of the operations succeeded");
                    else break;

                case FoldType.Any:
                    if (!ops.Any(_op => _op.Succeeded == true))
                        throw new Exception("None of the operations succeeded");
                    else break;

                default: throw new Exception("Invalid Fold State");
            }

            action.Invoke();
        });
        public static IOperation<Out> Then<Out>(this IEnumerable<IOperation> operations, Func<Out> func, FoldType foldType = FoldType.All) 
        => operations.Then(() => { }, foldType).Then(func);


        public static IOperation Then<In>(this IEnumerable<IOperation<In>> operations, Action<IEnumerable<In>> action, FoldType foldType = FoldType.All)
        => LazyOp.Try(() =>
        {
            var ops = operations.UsingEach(_op =>
            {
                try { _op.Resolve(); }
                catch { }
            })
            .ToArray();

            switch (foldType)
            {
                case FoldType.All:
                    if (!ops.All(_op => _op.Succeeded == true))
                        throw new AggregateException(ops.Where(_op => _op.Succeeded == false).Select(_op => _op.GetException()).ToArray());
                    else break;

                case FoldType.None:
                    if (ops.Any(_op => _op.Succeeded == true))
                        throw new Exception("Some of the operations succeeded");
                    else break;

                case FoldType.Any:
                    if (!ops.Any(_op => _op.Succeeded == true))
                        throw new Exception("None of the operations succeeded");
                    else break;

                default: throw new Exception("Invalid Fold State");
            }

            action.Invoke(ops.Where(_op => _op.Succeeded == true).Select(_op => _op.Result));
        });

        public static IOperation<Out> Then<In, Out>(this IEnumerable<IOperation<In>> operations, Func<IEnumerable<In>, Out> func, FoldType foldType = FoldType.All)
        => LazyOp.Try(() =>
        {
            var ops = operations.UsingEach(_op =>
            {
                try { _op.Resolve(); }
                catch { }
            })
            .ToArray();

            switch (foldType)
            {
                case FoldType.All:
                    if (!ops.All(_op => _op.Succeeded == true))
                        throw new AggregateException(ops.Where(_op => _op.Succeeded == false).Select(_op => _op.GetException()).ToArray());
                    else break;

                case FoldType.None:
                    if (ops.Any(_op => _op.Succeeded == true))
                        throw new Exception("Some of the operations succeeded");
                    else break;

                case FoldType.Any:
                    if (!ops.Any(_op => _op.Succeeded == true))
                        throw new Exception("None of the operations succeeded");
                    else break;

                default: throw new Exception("Invalid Fold State");
            }

            return func.Invoke(ops.Where(_op => _op.Succeeded == true).Select(_op => _op.Result));
        });
    }
}
