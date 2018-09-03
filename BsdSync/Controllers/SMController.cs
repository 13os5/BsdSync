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
    public class SMController : ApiController
    {
        _utilHelper helper = new _utilHelper();
 
        [HttpPost]
        [ActionName("CreateUser")]
        public IHttpActionResult CUser([FromBody] BsdUser user)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);   
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_SetNewBSDUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Full_name", user.Full_name);
                cmd.Parameters.AddWithValue("@Location_ID", user.Location_ID);
                cmd.Parameters.AddWithValue("@UserType", user.UserType);
                cmd.Parameters.AddWithValue("@CreateBy", user.Username);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                var Status = dt;
                #endregion
                return Json<DataTable>(Status);
            }
            catch( Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);

            }
        }

        [HttpPost]
        //[HttpGet]
        [ActionName("UserType")]
        public IHttpActionResult UserType(URD urd)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_GetUserType", con);
                cmd.Parameters.AddWithValue("@URD", urd.uRD);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                return Json<DataTable>(dt);
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }

        [HttpPost]
        [ActionName("GetUserPermission")]
        public IHttpActionResult GetUserPermission([FromBody] GetPermission user)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_GetUserPermission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", user.Username);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                var Status = dt;
                #endregion
                return Json<DataTable>(Status);
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);

            }
        }

        [HttpPost]
        [ActionName("InsertPermission")]
        public IHttpActionResult CUserPermission([FromBody] BsdInsertUserPermission userP)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_InsertBSDUsersPermission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", userP.Username);
                cmd.Parameters.AddWithValue("@chkBS", userP.chkBS);
                cmd.Parameters.AddWithValue("@chkCR", userP.chkCR);
                cmd.Parameters.AddWithValue("@chkPC", userP.chkPC);
                cmd.Parameters.AddWithValue("@chkPMP", userP.chkPMP);
                cmd.Parameters.AddWithValue("@chkRDM", userP.chkRDM);
                cmd.Parameters.AddWithValue("@chkRFC", userP.chkRFC);
                cmd.Parameters.AddWithValue("@chkRMP", userP.chkRMP);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                var Status = dt;
                #endregion
                return Json<DataTable>(Status);
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }

        [HttpPost]
        [ActionName("UpdatePermission")]
        public IHttpActionResult UUserPermission([FromBody] BsdUpdateUserPermission userP)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_UpdateBSDUsersPermission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", userP.Username);
                cmd.Parameters.AddWithValue("@UpdateBy", userP.UpdateBy);
                cmd.Parameters.AddWithValue("@chkBS", userP.chkBS);
                cmd.Parameters.AddWithValue("@chkCR", userP.chkCR);
                cmd.Parameters.AddWithValue("@chkPC", userP.chkPC);
                cmd.Parameters.AddWithValue("@chkPMP", userP.chkPMP);
                cmd.Parameters.AddWithValue("@chkRDM", userP.chkRDM);
                cmd.Parameters.AddWithValue("@chkRFC", userP.chkRFC);
                cmd.Parameters.AddWithValue("@chkRMP", userP.chkRMP);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                var Status = dt;
                #endregion
                return Json<DataTable>(Status);
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }

        [HttpPost]
        [ActionName("Login")]
        public IHttpActionResult Login([FromBody] UserLogin user)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_bsdMALogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usn", user.Username);
                cmd.Parameters.AddWithValue("@pwd", user.Password);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                //var Status = dt;
                #endregion
                return Json<DataTable>(dt);
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }

        [HttpPost]
        [ActionName("GetUser")]
        public IHttpActionResult User(URD URD)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_BSDServicegetUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@URD", URD.uRD);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                //var Status = dt;
                #endregion
                return Json<DataTable>(dt);
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }

        [HttpPost]
        [ActionName("ChangeImei")]
        public IHttpActionResult URoute([FromBody] Imei imei)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_Update_RouteSM", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@imeiOld", imei.oldimei.Trim());
                cmd.Parameters.AddWithValue("@imeiNew", imei.newimei.Trim());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                var Status = dt;
                #endregion
                return Json<DataTable>(Status);
            }
            catch( Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }

        [HttpPost]
        [ActionName("ChangeCon")]
        public IHttpActionResult UCon([FromBody] Con cons)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                StringBuilder builder3 = new StringBuilder();
                builder3.Append("<req>");
                builder3.Append("<info>");
                builder3.Append(string.Format("<u_id>{0}</u_id>", "SAMARTK"));
                builder3.Append(string.Format("<w_no>{0}</w_no>", cons.w_no.Trim()));
                builder3.Append(string.Format("<r_no>{0}</r_no>", cons.r_no.Trim()));
                builder3.Append("</info>");
                builder3.Append("</req>");
                string reqXml = builder3.ToString();
                SqlCommand cmd = new SqlCommand("sp_BSDH019_FixWrongScan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@xml", reqXml.Replace("&", " and "));
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string Result = dt.Rows[0][0].ToString();
                Result = Result.ToString().Replace("info", "").Replace("res", "");
                Result = Result.ToString().Replace("<", "").Replace("status", "");
                Result = Result.ToString().Replace("code", "").Replace("desc", "");
                Result = Result.ToString().Replace("Not found", "").Replace("/", "");
                Result = Result.ToString().Replace("FAILURE", "").Replace("SUCCEED", "");
                Result = Result.ToString().Replace("<", "").Replace(">", "");
                Result = Result.ToString().Replace("//", "");
                con.Close();

                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add(Result);
                return Json<DataTable>(dtNew);
            }
            catch( Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }


        [HttpGet]
        [ActionName("GetJobSMList")]
        public IHttpActionResult gList(string Lead,string ShopType)
        {
            try
            {
                SqlConnection con = new SqlConnection(helper.Strcon);
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_Get_SMJobLists", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Lead", Lead);
                cmd.Parameters.AddWithValue("@ShopType", ShopType);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                #region
                var Status = dt;
                #endregion
                return Json<DataTable>(Status);
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }


    }
}

                    public class URD
                    {
                        public bool uRD { get; set; }
                    }

                    public class BsdUser
                    {
                        public string Username { get; set; }
                        public string Password { get; set; }
                        public string Full_name { get; set; }
                        public string Location_ID { get; set; }
                        public string UserType { get; set; }
                    }

                    public class GetPermission
                    {
                        public string Username { get; set; }
                    }

                    public class BsdInsertUserPermission
                    {
                        public string Username { get; set; }
                        public bool chkBS { get; set; }
                        public bool chkCR { get; set; }
                        public bool chkPC { get; set; }
                        public bool chkPMP { get; set; }
                        public bool chkRDM { get; set; }
                        public bool chkRFC { get; set; }
                        public bool chkRMP { get; set; }
                    }

                    public class BsdUpdateUserPermission
                    {
                        public string Username { get; set; }
                        public string UpdateBy { get; set; }
                        public bool chkBS { get; set; }
                        public bool chkCR { get; set; }
                        public bool chkPC { get; set; }
                        public bool chkPMP { get; set; }
                        public bool chkRDM { get; set; }
                        public bool chkRFC { get; set; }
                        public bool chkRMP { get; set; }
}

                    public class Imei
                    {
                        public string oldimei { get; set; }
                        public string newimei { get; set; }
                    }


                    public class Con
                    {
                        public string u_id { get; set; }
                        public string w_no { get; set; }
                        public string r_no { get; set; }
                    }

                    public class UserLogin
                    {
                        public string Username { get; set; }
                        public string Password { get; set; }
                    }
//PROCEDURE[dbo].[sp_SetNewBSDUsers]
//@Username AS NVARCHAR(50),
//@Password AS NVARCHAR(20),
//@Full_name AS NVARCHAR(100),
//@Location_ID AS NVARCHAR(10)
//AS
//BEGIN

//        IF NOT EXISTS(SELECT* FROM tb_user_master
//                        where username = RTRIM(@Username))

//                        BEGIN
//                            INSERT INTO tb_user_master(username, [password], full_name, location_id, created_by, created_date)

//                            VALUES(@Username, @Password, @Full_name, @Location_ID,'SYSTEM', GETDATE())

//                            SELECT '200' AS Result

//                            RETURN
//                        END

//                        ELSE
//                            SELECT '000' AS Result
//END


//CREATE PROCEDURE sp_Update_RouteSM
//@imeiOld As NVARCHAR(MAX),
//@imeiNew As NVARCHAR(MAX)
//AS
//BEGIN

//    DECLARE @route_code AS NVARCHAR(35),@user_name AS NVARCHAR(35) , @password AS NVARCHAR(35)

//        IF NOT EXISTS(select* from tb_mobile_register where imei = @imeiNew)

//            BEGIN
//						--Set @route_code = (select route_code from tb_mobile_register where imei =  @imeiOld)
//						--Set @user_name = (select[user_name] from tb_mobile_register where imei = @imeiOld)
//						--Set @password = (select[password] from tb_mobile_register where imei = @imeiOld)
//						INSERT INTO tb_mobile_register
//                        SELECT @imeiNew,NULL,GETDATE(), route_code,user_name,password,status,reg_id,NULL,NULL,NULL,NULL,GETDATE()

//                        FROM tb_mobile_register where imei = @imeiOld


//                        UPDATE tb_mobile_register Set route_code = NULL,
//                        user_name = null, password = null, status = 0

//                        WHERE imei = @imeiOld

//                        SELECT '007' AS Result

//            END
//            ELSE

//                    SELECT '000' AS Result
//END


//SELECT B.MasterJ AS Shop, A.[shop - name] AS ShopName , A.boxes AS Pieces FROM

// (select h.origin_location_id as shop, t.location_type_name as [shop - name]

//, count(distinct s.consignment_no) as [boxes]
//from tb_batch_head as h(nolock) inner join tb_batch_detail as d(nolock) on h.batch_no = d.batch_no
//and convert(date, h.created_date) = convert(date, getdate())

//and h.status_code = 'SIP' inner join tb_route as r (nolock)
//on r.route_code = h.route_code and r.route_type = 'SHOP'

//inner join tb_location as l (nolock) on h.origin_location_id = l.location_id

//and l.location_type_id in (3,4) inner join tb_location_type as t on l.location_type_id = t.location_type_id
//inner join tb_shipment as s(nolock)

//    on d.consignment_no = s.consignment_no group by h.origin_location_id, t.location_type_name )  A
//     JOIN(SELECT TypeDesc As MasterJ FROM tb_SMLeadDetails Where[ObjID]
//    IN (SELECT[ObjID] FROM tb_SMLead

//    Where LeadName = RTRIM(@Lead) and TypeName = RTRIM(@ShopType))  ) B ON A.shop = B.MasterJ