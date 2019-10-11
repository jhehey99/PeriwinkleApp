using System;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace PeriwinkleApp.Core.Sources.Utils
{
    public static class FloatUtils
    {
        public static bool Equal (float? a, float? b, float epsilon = 0.00000001f)
        {
            // can null == null
            if (a == b)
                return true;

            // can't null == null, because previous if
            if (a == null || b == null)
                return false;

            float? absA = (a < 0) ? a *= -1 : a;
            float? absB = (b < 0) ? b *= -1 : b;

            float? diff = a - b;
            float? absDiff = (diff < 0) ? diff *= -1 : diff;

            if (a == 0 || b == 0 || absDiff < float.Epsilon)
                return absDiff < epsilon;
            
            return (absDiff / (absA + absB)) < epsilon;
        }
    }
}
