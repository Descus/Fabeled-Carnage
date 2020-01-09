namespace Utility
{
    public class Utilities
    {
        public static T[] ShiftDown<T>(T[] array, int index)
        {
            T[] ret = (T[]) array.Clone();
            for (int i = ret.Length - 1; i > index; i--)
            {
                ret[i] = ret[i - 1];
            }

            ret[index] = default;
            return ret;
        }
        
        public static T[] ShiftDownGetLast<T>(T[] array, int index, out T lastValue)
        {
            T[] ret = (T[]) array.Clone();
            lastValue = default;
            for (int i = ret.Length - 1; i >= index; i--)
            {
                if (i + 1 != ret.Length)
                {
                    ret[i] = ret[i + 1];
                }
                else
                {
                    lastValue = ret[i];
                }
            }
            return ret;
        }
        
    }
}