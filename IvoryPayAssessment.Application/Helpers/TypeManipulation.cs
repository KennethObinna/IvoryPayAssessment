using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Helpers
{
    public static class TypeManipulation
    {
        public static int StringToInt(this string value)
        {
            if(string.IsNullOrEmpty(value)) return 0;
            bool data=int.TryParse(value,out int result);

            if(data) return result;
            else return 0;
        }
        public static string ObjectToSring(this object value)
        {
            if( value is null) return string.Empty;
         string   result  =Convert.ToString(value);

            if(!string.IsNullOrWhiteSpace(result)) return result;
            else return string.Empty;
        }
        
    }
}
