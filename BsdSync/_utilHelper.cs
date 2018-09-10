using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BsdServiceSync
{
    public class _utilHelper
    {

        public string Strcon = ConfigurationManager.ConnectionStrings["BSDService"].ConnectionString;


        public string getColumnValueAsString(DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value && row[col] != null ? row[col].ToString() : string.Empty;
        }

        public int getColumnValueAsInteger(DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value && row[col] != null ? Convert.ToInt32(row[col]) : 0;
        }

        public double getColumnValueAsDouble(DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value && row[col] != null ? Convert.ToDouble(row[col]) : 0;
        }
        public string Escape(string sql)
        {
            return sql.Replace(@"/'", @" ").Replace(@"'", @"''");
        }
    }
}