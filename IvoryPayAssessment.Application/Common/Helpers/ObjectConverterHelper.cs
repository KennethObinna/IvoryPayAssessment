namespace IvoryPayAssessment.Application.Common.Helpers
{
    public static class ObjectConverterHelper
    {
        public static int ConvertToInt(this object obj)
        {
            if (obj is null) return 0;

            int data = Convert.ToInt32(obj);
            return data;
        }
        public static int ConvertToInt(this string obj)
        {
            if (string.IsNullOrEmpty(obj)) return 0;

            int data = Convert.ToInt32(obj);
            return data;
        }


        public static long ConvertToLong(this object obj)
        {
            if (obj is null) return 0;

            long data = Convert.ToInt64(obj);
            return data;
        }

        public static DBNull ConvertToDbNull(this object obj)
        {
            return DBNull.Value;

        }
        public static decimal ConvertToDecimal(this object obj)
        {
            if (obj is null) return 0m;

            decimal data = Convert.ToDecimal(obj);
            return data;
        }
  public static string ConvertToString(this object obj)
        {
            if (obj is null) return string.Empty;

            string data = Convert.ToString(obj);
            return data;
        }
        public static double ConvertToDouble(this object obj)
        {
            if (obj is null) return 0d;

            double data = Convert.ToDouble(obj);
            return data;
        }

        public static bool ConvertToBoolean(this object obj)
        {
            if (obj is null) return false;

            bool data = Convert.ToBoolean(obj);
            return data;
        }

    }
}
