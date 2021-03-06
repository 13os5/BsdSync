﻿using System;
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

namespace BsdServiceSync.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BsdServiceSyncController : ApiController
    {
        _utilHelper helper = new _utilHelper();

        [HttpGet]
        [ActionName("Admin")]
        public IHttpActionResult adminList()
        {
            var response = checkRequst();

            if (response.result)
            {
                using (DBManage dbm = new DBManage())
                {
                    try
                    {
                        return Json<DataTable>(dbm.ExecuteDataTableStored(new SqlCommand("sp_BSD_Get_adminList")));
                    }
                    catch (Exception ex)
                    {
                        return Json<string>(Convert.ToString(ex));
                    }
                }
            }
            else if (response.res == "400")
            {
                return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
            }
            else //401
            {
                return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
            }
        }

        [HttpGet]
        [ActionName("Display")]
        public IHttpActionResult adminList(string CreateBy, string StartDate, string EndDate)
        {
            var response = checkRequst();

            if (response.result)
            {
                using (DBManage dbm = new DBManage())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_BSD_GetReport"))
                    {
                        try
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@CreateBy", CreateBy.ToString().Trim());
                            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToString().Trim());
                            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToString().Trim());
                            return Json<DataTable>(dbm.ExecuteDataTableStored(cmd));
                        }
                        catch (Exception ex)
                        {
                            return Json<string>(Convert.ToString(ex));
                        }
                    }
                }
            }
            else if (response.res == "400")
            {
                return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
            }
            else //401
            {
                return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
            }
        }

        [HttpPost]
        [ActionName("CreateUser")]
        public IHttpActionResult CUser([FromBody] BsdUser user)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    using (DBManage dbm = new DBManage())
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_SetNewBSDServiceUsers"))
                        {
                            try
                            {
                                cmd.Parameters.AddWithValue("@Username", user.Username);
                                cmd.Parameters.AddWithValue("@Password", user.Password);
                                cmd.Parameters.AddWithValue("@Full_name", user.Full_name);
                                cmd.Parameters.AddWithValue("@Location_ID", user.Location_ID);
                                cmd.Parameters.AddWithValue("@UserType", user.UserType);
                                cmd.Parameters.AddWithValue("@CreateBy", user.CreateBy);
                                return Json<DataTable>(dbm.ExecuteDataTableStored(cmd));
                            }
                            catch (Exception ex)
                            {
                                return Json<string>(Convert.ToString(ex));
                            }
                        }
                    }
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("UserType")]
        public IHttpActionResult UserType(URD urd)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
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
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("GetUserPermission")]
        public IHttpActionResult GetUserPermission([FromBody] GetPermission user)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
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
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("InsertPermission")]
        public IHttpActionResult CUserPermission([FromBody] BsdInsertUserPermission userP)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
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
                    cmd.Parameters.AddWithValue("@chkPCP", userP.chkPCP);
                    cmd.Parameters.AddWithValue("@UpdateBy", userP.UpdateBy);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("UpdatePermission")]
        public IHttpActionResult UUserPermission([FromBody] BsdUpdateUserPermission userP)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
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
                    cmd.Parameters.AddWithValue("@chkPCP", userP.chkPCP);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("GetUserPermissionT")]
        public IHttpActionResult GetUserPermissionT([FromBody] GetPermission user)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_GetUserPermissionT", con);
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
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("InsertPermissionT")]
        public IHttpActionResult CUserPermissionT([FromBody] BsdInsertUserPermissionT userP)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_InsertBSDUsersPermissionT", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", userP.Username);
                    cmd.Parameters.AddWithValue("@tChkAP", userP.tChkAP);
                    cmd.Parameters.AddWithValue("@tChkCC", userP.tChkCC);
                    cmd.Parameters.AddWithValue("@tChkCR", userP.tChkCR);
                    cmd.Parameters.AddWithValue("@tChkCU", userP.tChkCU);
                    cmd.Parameters.AddWithValue("@tChkRBB", userP.tChkRBB);
                    cmd.Parameters.AddWithValue("@tChkRM", userP.tChkRM);
                    cmd.Parameters.AddWithValue("@tChkRP", userP.tChkRP);
                    cmd.Parameters.AddWithValue("@tChkSC", userP.tChkSC);
                    cmd.Parameters.AddWithValue("@tChkUM", userP.tChkUM);
                    cmd.Parameters.AddWithValue("@tChkUP", userP.tChkUP);
                    cmd.Parameters.AddWithValue("@tChkUPBS", userP.tChkUPBS);
                    cmd.Parameters.AddWithValue("@UpdateBy", userP.UpdateBy);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("UpdatePermissionT")]
        public IHttpActionResult UUserPermissionT([FromBody] BsdUpdateUserPermissionT userP)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_UpdateBSDUsersPermissionT", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", userP.Username);
                    cmd.Parameters.AddWithValue("@UpdateBy", userP.UpdateBy);
                    cmd.Parameters.AddWithValue("@tChkAP", userP.tChkAP);
                    cmd.Parameters.AddWithValue("@tChkCC", userP.tChkCC);
                    cmd.Parameters.AddWithValue("@tChkCR", userP.tChkCR);
                    cmd.Parameters.AddWithValue("@tChkCU", userP.tChkCU);
                    cmd.Parameters.AddWithValue("@tChkRBB", userP.tChkRBB);
                    cmd.Parameters.AddWithValue("@tChkRM", userP.tChkRM);
                    cmd.Parameters.AddWithValue("@tChkRP", userP.tChkRP);
                    cmd.Parameters.AddWithValue("@tChkSC", userP.tChkSC);
                    cmd.Parameters.AddWithValue("@tChkUM", userP.tChkUM);
                    cmd.Parameters.AddWithValue("@tChkUP", userP.tChkUP);
                    cmd.Parameters.AddWithValue("@tChkUPBS", userP.tChkUPBS);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("Login")]
        public IHttpActionResult Login([FromBody] UserLogin user)
        {
            UserLogin Permission = new UserLogin();
            Permission.AddProfile = false;
            Permission.ChangeCons = false;
            Permission.ChangeRoute = false;
            Permission.CreateUser = false;
            Permission.RptBsdBooking = false;
            Permission.RptMsp = false;
            Permission.RptProfile = false;
            Permission.SipCons = false;
            Permission.UpdateMobile = false;
            Permission.UpdatePermission = false;
            Permission.UpdatePermissionTool = false;
            Permission.AddConToBatch = false;

            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_bsdMALoginTools", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usn", user.Username);
                    cmd.Parameters.AddWithValue("@pwd", user.Password);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    //DBManage db = new DBManage();
                    //db.Dispose();

                    //dt = new DataTable();

                    if (dt.Rows.Count > 0)
                    {
                        Permission.Role = dt.Rows[0]["role"].ToString();
                        if (dt.Rows[0]["Permit"].ToString() != "0")
                        {
                            int c = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                switch (dt.Rows[c]["Permission"].ToString())
                                {
                                    case "AddProfile":
                                        Permission.AddProfile = true;
                                        break;
                                    case "ChangeCons":
                                        Permission.ChangeCons = true;
                                        break;
                                    case "ChangeRoute":
                                        Permission.ChangeRoute = true;
                                        break;
                                    case "CreateUser":
                                        Permission.CreateUser = true;
                                        break;
                                    case "RptBsdBooking":
                                        Permission.RptBsdBooking = true;
                                        break;
                                    case "RptMsp":
                                        Permission.RptMsp = true;
                                        break;
                                    case "RptProfile":
                                        Permission.RptProfile = true;
                                        break;
                                    case "SipCons":
                                        Permission.SipCons = true;
                                        break;
                                    case "UpdateMobile":
                                        Permission.UpdateMobile = true;
                                        break;
                                    case "UpdatePermission":
                                        Permission.UpdatePermission = true;
                                        break;
                                    case "UpdatePermissionTool":
                                        Permission.UpdatePermissionTool = true;
                                        break;
                                    case "AddConToBatch":
                                        Permission.AddConToBatch = true;
                                        break;
                                }
                                c++;
                            }
                        }
                    }

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
                        string token = genToken(dt.Rows[0]["username"].ToString(), dt.Rows[0]["Permit"].ToString(), Permission);
                        dtLogin.Rows.Add(true, token);
                        return Json<DataTable>(dtLogin);
                    }
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        public Auth checkRequst()
        {
            Auth auth = new Auth();
            string appId = string.Empty;
            string appKey = string.Empty;
            string id = "BSD_Service";
            string key = "WarakornT13os5!#";

            Request.Headers.Contains("app_id");
            Request.Headers.Contains("app_key");
            try
            {
                appId = Request.Headers.GetValues("app_id").First();
                appKey = Request.Headers.GetValues("app_key").First();
            }
            catch (Exception ex)
            {
                auth.result = false;
                auth.res = "400";
            }

            if (String.IsNullOrEmpty(appId) || String.IsNullOrEmpty(appKey))
            {
                auth.result = false;
                auth.res = "400";
            }
            else
            {
                appId = Request.Headers.GetValues("app_id").First();
                appKey = Request.Headers.GetValues("app_key").First();

                if (appId != id || appKey != key)
                {
                    auth.result = false;
                    auth.res = "401";
                }
                else
                {
                    auth.result = true;
                    auth.res = "200";
                }
            }

            return auth;
        }

        public string genToken(string usn, string Permit, UserLogin Permission)
        {
            string key = " WarakornT.!#~ WarakornT.!~% WarakornT.!@~ WarakornT.!!~ WarakornT.!&~ WarakornT.!~~!@#!";

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            var payload = new JwtPayload
            {
               { "name", usn },
               { "role", Permission.Role },
               { "AddProfile", Permission.AddProfile },
               { "ChangeCons", Permission.ChangeCons },
               { "ChangeRoute", Permission.ChangeRoute },
               { "CreateUser", Permission.CreateUser },
               { "RptBsdBooking", Permission.RptBsdBooking },
               { "RptMsp", Permission.RptMsp },
               { "RptProfile", Permission.RptProfile },
               { "SipCons", Permission.SipCons },
               { "UpdateMobile", Permission.UpdateMobile },
               { "UpdatePermission", Permission.UpdatePermission },
               { "UpdatePermissionTool", Permission.UpdatePermissionTool },
               { "AddConToBatch", Permission.AddConToBatch }
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
                var response = checkRequst();

                if (response.result)
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
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
                }
            }
            catch (Exception ex)
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Result", typeof(string));
                dtNew.Rows.Add("Error : " + ex.ToString());
                return Json<DataTable>(dtNew);
            }
            //using (DBManage db = new DBManage())
            //{
            //    ResponseResult result = new ResponseResult();
            //    try
            //    {
            //        var response = checkRequst();
            //        if (response.result)
            //        {
            //            SqlCommand cmd = new SqlCommand("sp_BSDServicegetUser");
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.AddWithValue("@URD", URD.uRD);

            //            DataTable dt = new DataTable();

            //            dt = db.ExecuteDataTable(cmd);
            //            if (dt.Rows.Count > 0)
            //            {
            //                result.result = true;
            //                result.errorcode = "";
            //                result.errormessage = "";
            //                result.dataresult = dt;
            //            }
            //            else
            //            {

            //            }

            //            return Json<DataTable>(dt);
            //        }
            //        else if (response.res == "400")
            //        {
            //            return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
            //        }
            //        else //401
            //        {
            //            return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        DataTable dtNew = new DataTable();
            //        dtNew.Columns.Add("Result", typeof(string));
            //        dtNew.Rows.Add("Error : " + ex.ToString());
            //        return Json<DataTable>(dtNew);
            //    }
            //    finally
            //    {
            //        return Json<ResponseResult>(result);
            //    }
            //}
        }

        [HttpPost]
        [ActionName("GetCons")]
        public IHttpActionResult GetCons(Cons Cons)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_getSearchCon", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@con_no", Cons.ConsNo);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("ChangeImei")]
        public IHttpActionResult URoute([FromBody] Imei imei)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
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
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("ChangeCon")]
        public IHttpActionResult UCon([FromBody] Con cons)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    StringBuilder builder3 = new StringBuilder();
                    builder3.Append("<req>");
                    builder3.Append("<info>");
                    builder3.Append(string.Format("<u_id>{0}</u_id>", cons.u_id.Trim()));
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
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("GetRoute")]
        public IHttpActionResult GetRoute([FromBody] Route route)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_GetRoute", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RouteCode", route.RouteCode == null ? "" : route.RouteCode);
                    cmd.Parameters.AddWithValue("@Team", route.Team == null ? "" : route.Team);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpGet]
        [ActionName("GetTeam")]
        public IHttpActionResult GetTeam()
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_ChangeRouteGetTeam", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpGet]
        [ActionName("GetLocationAll")]
        public IHttpActionResult GetLocationAll()
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_GetlocationAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpGet]
        [ActionName("GetZone")]
        public IHttpActionResult GetZone()
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_GetZone", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("ChangeRoute")]
        public IHttpActionResult ChangeRoute([FromBody] Route route)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_ChangeRoute", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RouteCode", route.RouteCode == null ? "" : route.RouteCode);
                    cmd.Parameters.AddWithValue("@OriginLocationId", route.OriginLocalId == null ? "" : route.OriginLocalId);
                    cmd.Parameters.AddWithValue("@Username", route.Uname);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpGet]
        [ActionName("GetPayerType")]
        public IHttpActionResult GetPayerType()
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_AddProfileGetPayerId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("AddProfile")]
        public IHttpActionResult AddProfile([FromBody] Profile profile)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_AddProfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@profile_id", profile.profile_id);
                    cmd.Parameters.AddWithValue("@profile_name", profile.profile_name);
                    cmd.Parameters.AddWithValue("@payer_id", profile.payer_id);
                    cmd.Parameters.AddWithValue("@booking_type", profile.booking_type);
                    cmd.Parameters.AddWithValue("@sender_name", profile.sender_name);
                    cmd.Parameters.AddWithValue("@sender_address1", profile.sender_address1);
                    cmd.Parameters.AddWithValue("@sender_address2", profile.sender_address2);
                    cmd.Parameters.AddWithValue("@sender_zipcode", profile.sender_zipcode);
                    cmd.Parameters.AddWithValue("@sender_mobile", profile.sender_mobile);
                    cmd.Parameters.AddWithValue("@profile_skipped", profile.profile_skipped);
                    cmd.Parameters.AddWithValue("@profile_restricted", profile.profile_restricted);
                    cmd.Parameters.AddWithValue("@sender_idcard", profile.sender_idcard == null ? "" : profile.sender_idcard);
                    cmd.Parameters.AddWithValue("@create_by", profile.Uname);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("GetInFoData")]
        public IHttpActionResult GetInFoData([FromBody] SIPCons SipCons)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_SIPConsignmentGetInfoData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingNo", SipCons.BookingNo);
                    cmd.Parameters.AddWithValue("@Date", SipCons.Date);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("SIPCons")]
        public IHttpActionResult SIPCons([FromBody] SIPCons SipCons)
        {
            try
            {
                var response = checkRequst();
                DataTable Status = new DataTable();
                using (DBManage dbm = new DBManage(true))
                {
                    if (response.result)
                    {

                        using (SqlCommand cmd = new SqlCommand("sp_Bsd_SIPConsignment"))
                        {
                            try
                            {
                                string[] txtSplit = SipCons.RecordId.Split(',');
                                for (int i = 0; i < txtSplit.Length; i++)
                                {
                                    string x = Convert.ToString(txtSplit[i]);
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@RecordId", Convert.ToString(txtSplit[i]));
                                    Status = dbm.ExecuteDataTableStored(cmd);
                                }
                                dbm.TransactionCommit();
                            }
                            catch (Exception ex)
                            {
                                dbm.TransactionRollBack();
                                return Json<string>(Convert.ToString(ex));
                            }
                        }
                    }
                    else if (response.res == "400")
                    {
                        return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                    }
                    else //401
                    {
                        return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
                    }
                }
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

        [HttpGet]
        [ActionName("GetRouteAll")]
        public IHttpActionResult GetRouteAll()
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_BsdUpdateMobile_GetRoute", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("UpdateMobile")]
        public IHttpActionResult UpdateMobile([FromBody] UpdateMobile UMobile)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_UpdateMobile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Imei", UMobile.Imei);
                    cmd.Parameters.AddWithValue("@RouteCode", UMobile.RouteCode);
                    cmd.Parameters.AddWithValue("@IsDelete", UMobile.IsDelete);
                    cmd.Parameters.AddWithValue("@Create_by", UMobile.Uname);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("ChkDupUMobile")]
        public IHttpActionResult ChkDupUMobile([FromBody] UpdateMobile UMobile)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_UpdateMobileChkDuplicate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RouteCode", UMobile.RouteCode);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    var Status = dt;
                    #endregion
                    return Json<DataTable>(Status);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpGet]
        [ActionName("GetRoute")]
        public IHttpActionResult GetRoute()
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_UpdateMobileGetRouteCode", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpGet]
        [ActionName("GetImeiList")]
        public IHttpActionResult GetImeiList()
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_UpdateMobileGetimeilist", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    #region
                    //var Status = dt;
                    #endregion
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("GetRptMSP")]
        public IHttpActionResult GetRptMSP([FromBody] RptMSP RptMsp)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_ReportMSP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileId", RptMsp.ProfileId);
                    cmd.Parameters.AddWithValue("@DaetFrom", RptMsp.DateFrom);
                    cmd.Parameters.AddWithValue("@DaetTo", RptMsp.DateTo);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("GetRptProfile")]
        public IHttpActionResult GetRptProfile([FromBody] RptProfile RptP)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_ReportProfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SenderMobile", RptP.SenderMobile);
                    cmd.Parameters.AddWithValue("@DaetFrom", RptP.DateFrom);
                    cmd.Parameters.AddWithValue("@DaetTo", RptP.DateTo);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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

        [HttpPost]
        [ActionName("AddConToBatch")]
        public IHttpActionResult AddConToBatch([FromBody] AddConToBatch actb)
        {
            try
            {
                var response = checkRequst();

                if (response.result)
                {
                    SqlConnection con = new SqlConnection(helper.Strcon);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("sp_Bsd_Addcontobatch", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Batch_no", actb.batchNo);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    return Json<DataTable>(dt);
                }
                else if (response.res == "400")
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.BadRequest)));
                }
                else //401
                {
                    return Json<string>(Convert.ToString(Request.CreateResponse(HttpStatusCode.Unauthorized)));
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
    }
}

public class Auth
{
    public bool result { get; set; }
    public string res { get; set; }
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
    public Int64 UserType { get; set; }
    public string CreateBy { get; set; }
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
    public bool chkPCP { get; set; }
    public string UpdateBy { get; set; }
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
    public bool chkPCP { get; set; }
    public bool chkRMP { get; set; }
}

public class BsdInsertUserPermissionT
{
    public string Username { get; set; }
    public bool tChkAP { get; set; }
    public bool tChkCC { get; set; }
    public bool tChkCR { get; set; }
    public bool tChkCU { get; set; }
    public bool tChkRBB { get; set; }
    public bool tChkRM { get; set; }
    public bool tChkRP { get; set; }
    public bool tChkSC { get; set; }
    public bool tChkUM { get; set; }
    public bool tChkUP { get; set; }
    public bool tChkUPBS { get; set; }
    public string UpdateBy { get; set; }
}

public class BsdUpdateUserPermissionT
{
    public string Username { get; set; }
    public string UpdateBy { get; set; }
    public bool tChkAP { get; set; }
    public bool tChkCC { get; set; }
    public bool tChkCR { get; set; }
    public bool tChkCU { get; set; }
    public bool tChkRBB { get; set; }
    public bool tChkRM { get; set; }
    public bool tChkRP { get; set; }
    public bool tChkSC { get; set; }
    public bool tChkUM { get; set; }
    public bool tChkUP { get; set; }
    public bool tChkUPBS { get; set; }
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
    public bool AddProfile { get; set; }
    public bool ChangeCons { get; set; }
    public bool ChangeRoute { get; set; }
    public bool CreateUser { get; set; }
    public bool RptBsdBooking { get; set; }
    public bool RptMsp { get; set; }
    public bool RptProfile { get; set; }
    public bool SipCons { get; set; }
    public bool UpdateMobile { get; set; }
    public bool UpdatePermission { get; set; }
    public bool UpdatePermissionTool { get; set; }
    public bool AddConToBatch { get; set; }
    public string Role { get; set; }
}

public class Cons
{
    public string ConsNo { get; set; }
}

public class Route
{
    public string RouteCode { get; set; }
    public string Team { get; set; }
    public string OriginLocalId { get; set; }
    public string Uname { get; set; }
}

public class Zones
{
    public string Zone { get; set; }
}

public class Profile
{
    public string profile_id { get; set; }
    public string profile_name { get; set; }
    public string payer_id { get; set; }
    public string booking_type { get; set; }
    public string sender_name { get; set; }
    public string sender_address1 { get; set; }
    public string sender_address2 { get; set; }
    public string sender_zipcode { get; set; }
    public string sender_mobile { get; set; }
    public string profile_skipped { get; set; }
    public string profile_restricted { get; set; }
    public string sender_idcard { get; set; }
    public string Uname { get; set; }
}

public class SIPCons
{
    public string BookingNo { get; set; }
    public string Date { get; set; }
    public string RecordId { get; set; }
}

public class UpdateMobile
{
    public string Imei { get; set; }
    public string RouteCode { get; set; }
    public bool IsDelete { get; set; }
    public string Uname { get; set; }
}

public class RptMSP
{
    public string ProfileId { get; set; }
    public string DateFrom { get; set; }
    public string DateTo { get; set; }
}

public class RptProfile
{
    public string SenderMobile { get; set; }
    public string DateFrom { get; set; }
    public string DateTo { get; set; }
}

public class AddConToBatch
{
    public string batchNo { get; set; }
}

//public class ResponseResult
//{
//    public bool result = false;
//    public string errorcode = "";
//    public string errormessage = "";
//    public object dataresult;
//}