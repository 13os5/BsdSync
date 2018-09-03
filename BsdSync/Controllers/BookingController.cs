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
    public class BookingController : ApiController
    {
        _utilHelper helper = new _utilHelper();
        [HttpPost]
        [ActionName("Pcan")]
        public IHttpActionResult BookingFromWinService([FromBody] CreateBooking2 bk)
        {
            SqlConnection con = new SqlConnection(helper.Strcon);
            con.Open();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("sp_BSD_CreateBooking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ref_no", bk.ref_no);
            cmd.Parameters.AddWithValue("@ord_no",  bk.ord_no);
            cmd.Parameters.AddWithValue("@type_id", bk.type_id);
            cmd.Parameters.AddWithValue("@payerid", bk.payerid);
            cmd.Parameters.AddWithValue("@payable_type", bk.payable_type);
            cmd.Parameters.AddWithValue("@sender_name", bk.sender_name);
            cmd.Parameters.AddWithValue("@sender_address1", bk.sender_address1);
            cmd.Parameters.AddWithValue("@sender_address2", bk.sender_address2);
            cmd.Parameters.AddWithValue("@sender_contact_person", bk.sender_contact_person);
            cmd.Parameters.AddWithValue("@sender_zipcode", bk.sender_zipcode);
            cmd.Parameters.AddWithValue("@sender_mobile", bk.sender_mobile);
            cmd.Parameters.AddWithValue("@req_pickup_datetime", bk.req_pickup_datetime);
            cmd.Parameters.AddWithValue("@req_pickup_latitude", bk.req_pickup_latitude);
            cmd.Parameters.AddWithValue("@req_pickup_longitude", bk.req_pickup_longitude);
            cmd.Parameters.AddWithValue("@remark", bk.remark);
            cmd.Parameters.AddWithValue("@req_total_pkg", bk.req_total_pkg);
            cmd.Parameters.AddWithValue("@req_total_wt", bk.req_total_wt);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            #region
            var bookingReturn = dt;
             #endregion
            return Json<DataTable>(bookingReturn);

        }

        [HttpGet]
        [ActionName("Pcan2")]
        public IHttpActionResult UpdateStatus(string Ord_no,string Status_Code)
        {
            SqlConnection con = new SqlConnection(helper.Strcon);
            con.Open();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("sp_BSD_UpdateStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ord_no", Ord_no);
            cmd.Parameters.AddWithValue("@status_code", Status_Code);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return Json < DataTable >(dt);
        }

        [HttpPost]
        [ActionName("Create")]
        public IHttpActionResult BookingFromMobileReal([FromBody] CreateBooking bk)
        {

            SqlConnection con = new SqlConnection(helper.Strcon);
            con.Open();

            DbHelper db3 = new DbHelper("BsdDbConnection");
            StringBuilder builder3 = new StringBuilder();
            builder3.Append("<req>");
            builder3.Append("<info>");
            builder3.Append(string.Format("<userid>{0}</userid>", bk.pickup_route));
            builder3.Append(string.Format("<ref_no>{0}</ref_no>", bk.ref_no));//ROUTE
            builder3.Append(string.Format("<ord_no>{0}</ord_no>", bk.ord_no));
            builder3.Append(string.Format("<type_id>{0}</type_id>", "3"));
            builder3.Append(string.Format("<payer_id>{0}</payer_id>", "CITY"));//ROUTE Who SEnt
            builder3.Append("<payable_type>COP</payable_type>");
            builder3.Append(string.Format("<s_card>{0}</s_card>", bk.sender_idcard));
            builder3.Append(string.Format("<s_name>{0}</s_name>", bk.sender_name));
            builder3.Append(string.Format("<s_addr1>{0}</s_addr1>", bk.sender_address1));
            builder3.Append(string.Format("<s_addr2>{0}</s_addr2>", string.Format("{0} {1} {2}", "Lad", "Bangkhean", "Bangkok")));
            builder3.Append(string.Format("<s_contact>{0}</s_contact>", bk.sender_name));
            builder3.Append(string.Format("<s_zip>{0}</s_zip>", bk.sender_zipcode));
            builder3.Append(string.Format("<s_mobile>{0}</s_mobile>", bk.sender_mobile));
            builder3.Append(string.Format("<req_pup_date>{0}</req_pup_date>", ""));
            builder3.Append(string.Format("<lat>{0}</lat>", bk.req_pickup_latitude.ToString()));
            builder3.Append(string.Format("<lng>{0}</lng>", bk.req_pickup_longitude.ToString()));
            builder3.Append(string.Format("<remark>{0}</remark>", bk.remark));
            builder3.Append("<tot_pkg>1</tot_pkg>");//Always 1
            builder3.Append("<tot_wt>0</tot_wt>");
            builder3.Append("</info>");
            builder3.Append("</req>");
            string reqXml = builder3.ToString();

            //SqlParameter parameter =  new SqlParameter("@xml", SqlDbType.NVarChar) { Value = reqXml.Replace("&", " and ") };
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("sp_BSDG001_SetBookingInfo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@xml", reqXml.Replace("&", " and "));
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            string bookingReturn = dt.Rows[0][0].ToString();
            bookingReturn = bookingReturn.ToString().Replace("info", "").Replace("res", "");
            bookingReturn = bookingReturn.ToString().Replace("<", "").Replace(">", "");
            bookingReturn = bookingReturn.ToString().Replace("booking_no", "").Replace("/", "");
            bookingReturn = bookingReturn.ToString().Replace("//", "");
            return Json<string>(bookingReturn);

        }

        [HttpPost]
        [ActionName("Create2")]
        public IHttpActionResult BookingFromMobileTest([FromBody] CreateBooking bk)
        {
            SqlConnection con = new SqlConnection(helper.Strcon);
            con.Open();
            DbHelper db3 = new DbHelper("BsdDbConnection");
            StringBuilder builder3 = new StringBuilder();
            builder3.Append("<req>");
            builder3.Append("<info>");
            builder3.Append(string.Format("<userid>{0}</userid>", bk.pickup_route));
            builder3.Append(string.Format("<ref_no>{0}</ref_no>", bk.ref_no));//ROUTE
            builder3.Append(string.Format("<ord_no>{0}</ord_no>", bk.ord_no));
            builder3.Append(string.Format("<type_id>{0}</type_id>", "6"));
            builder3.Append(string.Format("<payer_id>{0}</payer_id>", ""));//ROUTE Who SEnt
            builder3.Append("<payable_type>COP</payable_type>");
            builder3.Append(string.Format("<s_card>{0}</s_card>", bk.sender_idcard));
            builder3.Append(string.Format("<s_name>{0}</s_name>", bk.sender_name));
            builder3.Append(string.Format("<s_addr1>{0}</s_addr1>", bk.sender_address1 ));
            builder3.Append(string.Format("<s_addr2>{0}</s_addr2>",   bk.sender_address2 ));
            builder3.Append(string.Format("<s_contact>{0}</s_contact>", bk.sender_name));
            builder3.Append(string.Format("<s_zip>{0}</s_zip>", bk.sender_zipcode));
            builder3.Append(string.Format("<s_mobile>{0}</s_mobile>", bk.sender_mobile));
            builder3.Append(string.Format("<req_pup_date>{0}</req_pup_date>", ""));
            builder3.Append(string.Format("<lat>{0}</lat>", bk.req_pickup_latitude.ToString()));
            builder3.Append(string.Format("<lng>{0}</lng>", bk.req_pickup_longitude.ToString()));
            builder3.Append(string.Format("<remark>{0}</remark>", bk.remark));
            builder3.Append("<tot_pkg>1</tot_pkg>");//Always 1
            builder3.Append("<tot_wt>0</tot_wt>");
            builder3.Append("</info>");
            builder3.Append("</req>");
            string reqXml = builder3.ToString();

            //SqlParameter parameter =  new SqlParameter("@xml", SqlDbType.NVarChar) { Value = reqXml.Replace("&", " and ") };
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("sp_BSDG001_SetBookingInfo_State", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@xml", reqXml.Replace("&", " and "));
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            
            string bookingReturn = dt.Rows[0][0].ToString();
            bookingReturn = bookingReturn.ToString().Replace("info", "").Replace("res", "");
            bookingReturn = bookingReturn.ToString().Replace("<", "").Replace(">", "");
            bookingReturn = bookingReturn.ToString().Replace("booking_no", "").Replace("/", "");
            bookingReturn = bookingReturn.ToString().Replace("//", "");



            if(bookingReturn != "" || bookingReturn != null)
            {
                cmd = new SqlCommand("sp_BSDG001_SetBookingInfo_State_DISP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@booking_no", bookingReturn);
                cmd.Parameters.AddWithValue("@remark", bk.remark);
                cmd.Parameters.AddWithValue("@latitude", "0");
                cmd.Parameters.AddWithValue("@longitude", "0");
                cmd.Parameters.AddWithValue("@created_by", bk.pickup_route);
                cmd.ExecuteNonQuery();
            }

            for (int x = 1;x<=2;x++)
            {
                string getDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string infoKey = "";
                if ( x == 1)
                {
                     infoKey = string.Format("{0}|{1}|{2}", bookingReturn, getDate, "PRQ");
                }
                if (x == 2)
                {
                    infoKey = string.Format("{0}|{1}|{2}", bookingReturn, getDate, "DISP");
                }
                cmd = new SqlCommand("sp_BSDG901_SaveTmpExport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@info_type", "booking_status");
                cmd.Parameters.AddWithValue("@info_key", infoKey);
                cmd.Parameters.AddWithValue("@for_system", "EDI");
                cmd.ExecuteNonQuery();
            }

            con.Close();
            return Json<string>(bookingReturn);
        }

        [HttpGet]
        [ActionName("getBookingAll")]
        public IHttpActionResult GetBookAll()
        {
            SqlConnection con = new SqlConnection(helper.Strcon);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from tb_booking_State", con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return Json<DataTable>(dt);

        }
 
    }
}
    public class CreateBooking
    {

        public string booking_no { get; set; }
        public string ref_no { get; set; }
        public string ord_no { get; set; }
        public string type_id { get; set; }
        public string payable_type { get; set; }
        public string payerid { get; set; }
        public string profile_id { get; set; }
        public string sender_idcard { get; set; }
        public string sender_name { get; set; }
        public string sender_address1 { get; set; }
 
        public string sender_address2 { get; set; }
        public string sender_zipcode { get; set; }
        public string sender_country { get; set; }
        public string sender_contact_person { get; set; }
        public string sender_mobile { get; set; }
        public string sender_phone { get; set; }
        public string sender_fax { get; set; }
        public string sender_email { get; set; }
        public string req_pickup_datetime { get; set; }
        public string req_pickup_location { get; set; }

        public string req_pickup_latitude { get; set; }
        public string req_pickup_longitude { get; set; }
        public string req_total_pkg { get; set; }
        public string req_total_wt { get; set; }
        public string req_total_dim_wt { get; set; }
        public string act_pickup_datetime { get; set; }
        public string pickup_route { get; set; }
        public string exception_code { get; set; }
        public string status_code { get; set; }
        public string status_datetime { get; set; }

        public string remark { get; set; }
        public string remark2 { get; set; }
        public string processed_flag { get; set; }
        public string processed_date { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string updated_by { get; set; }
        public string updated_date { get; set; }
        public string calling_by { get; set; }
        public string calling_date { get; set; }

        public string calling_flag { get; set; }
        public string reason_id { get; set; }
        public string copc_type { get; set; }
        public string copc_amount { get; set; }
        public string payment_gw_trans_no { get; set; }
        public string member_id { get; set; }
        public string cod_account_code { get; set; }
        public string cod_account_name { get; set; }
        public string cod_account_bank { get; set; }
        public string cod_type { get; set; }

        public string cod_amt { get; set; }
        public string cod_fee { get; set; }
        public string cod_vat { get; set; }
        public string tax_branch { get; set; }
        public string tax_id { get; set; }
        public string tax_type { get; set; }
        public string receipt_no { get; set; }
        public string invoice_no { get; set; }
        public string synchronize_pdc { get; set; }
        public string receipt_via_sms { get; set; }
  
}

    public class CreateBooking2
{

    public string ref_no { get; set; }
    public string ord_no { get; set; }

    public int type_id { get; set; }
    public string payerid { get; set; }

    public string payable_type { get; set; }
    public string sender_name { get; set; }

    public string sender_address1 { get; set; }
    public string sender_address2 { get; set; }

    public string sender_contact_person { get; set; }
    public string sender_zipcode { get; set; }

    public string sender_mobile { get; set; }
    public DateTime  req_pickup_datetime { get; set; }


    public string req_pickup_latitude { get; set; }
    public string req_pickup_longitude { get; set; }

    public string remark { get; set; }
    public string req_total_pkg { get; set; }


    public string req_total_wt { get; set; }

}






