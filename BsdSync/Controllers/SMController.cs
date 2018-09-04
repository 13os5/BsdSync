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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Web;
using IdentityModel.Client;

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
                Request.Headers.Contains("app_id");
                Request.Headers.Contains("app_key");
                string appId;
                string appKey;

                if (String.IsNullOrEmpty(Request.Headers.GetValues("app_id").First()) || String.IsNullOrEmpty(Request.Headers.GetValues("app_key").First()))
                {
                    return null;
                }
                else if (Request.Headers.GetValues("app_id").First() != "BSD_Service" || Request.Headers.GetValues("app_key").First() != " WarakornT13os5!#")
                {
                    return null;
                }
                else
                {
                    appId = Request.Headers.GetValues("app_id").First();
                    appKey = Request.Headers.GetValues("app_key").First();

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

                    #region
                    //var Status = dt;
                    #endregion
                    con.Close();

                    DataTable dtLogin = new DataTable();
                    dtLogin.Columns.Add("success", typeof(bool));
                    dtLogin.Columns.Add("token", typeof(string));

                    if (dt.Rows[0]["Permit"].ToString() == "0")
                    {
                        dtLogin.Rows.Add(false, null);
                        return Json<DataTable>(dtLogin);
                    }
                    else
                    {
                        string token = genToken(dt.Rows[0]["username"].ToString(), dt.Rows[0]["Permit"].ToString());
                        dtLogin.Rows.Add(true, token);
                        return Json<DataTable>(dtLogin);
                    }
                }
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
        }

        public string genToken(string usn, string Permit)
        {
            string key = " WarakornT.!#~ WarakornT.!~% WarakornT.!@~ WarakornT.!!~ WarakornT.!&~ WarakornT.!~~!@#!";

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            var payload = new JwtPayload
            {
               { "name", usn},
               { "role", Permit} // 0 = not permit, 1 = admin, 2 = RD, 3 = Extra user
            };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            var tokenString = handler.WriteToken(secToken);

            var token = handler.ReadJwtToken(tokenString);

            return token.RawData.ToString();
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