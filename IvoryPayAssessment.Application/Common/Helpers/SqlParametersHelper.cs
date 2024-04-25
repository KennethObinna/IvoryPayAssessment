using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.Helpers
{
    public class SqlParametersHelper
    {

        public static string SqlParameter(SqlParameter[] parameters)
        {
            string sql = string.Join(",",parameters.Select(p=>p.ParameterName));
            return sql;
        }
    }
}
