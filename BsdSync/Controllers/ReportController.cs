using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http.Cors;

namespace BsdSync.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        _utilHelper helper = new _utilHelper();

        [HttpGet]
        [ActionName("Admin")]
        public IHttpActionResult adminList()
        {
            SqlConnection con = new SqlConnection(helper.Strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_BSD_Get_adminList", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return Json<DataTable>(dt);
        }

        [HttpGet]
        [ActionName("Display")]
        public IHttpActionResult adminList(string CreateBy,string StartDate,string EndDate)
        {
            SqlConnection con = new SqlConnection(helper.Strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_BSD_GetReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@CreateBy", CreateBy.ToString().Trim());
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToString().Trim());
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToString().Trim());
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return Json<DataTable>(dt);
        }



    }
}
