namespace Common.Extensions
{
    public static class TupleExtensions
    {
        public static bool TryOut<TOut>(this ValueTuple<bool, TOut> tuple, out TOut outValue)
        {
            bool isSuccess;
            (isSuccess, outValue) = tuple;
            return isSuccess;
        }
    }
}
