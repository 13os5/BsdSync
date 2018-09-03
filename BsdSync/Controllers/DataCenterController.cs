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
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BsdSync.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class DataCenterController : ApiController
    {
        //[HttpPost]
        //[ActionName("getInfo")]
        //public IHttpActionResult get([FromBody] Mobile mb)
        //{
        //    DataTable dtJson = new DataTable();
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("https://rtsp.th.kerryexpress.com/masterdatawebapi/customer/detail");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.Add("app_id", "MDEAPP");
        //    client.DefaultRequestHeaders.Add("app_key", "0b937635df304629861e622378b93571");
        //    //string ParamValue = "abc";
        //    string jsonBody = $@"
        //        {{ 
        //            ""CustMobile"":""{mb.Mobile_no.ToString()}""
        //        }}";
        //    try
        //    {
        //        HttpResponseMessage response = client.PostAsync("https://rtsp.th.kerryexpress.com/masterdatawebapi/customer/detail", new StringContent(jsonBody, Encoding.UTF8, "application/json")).Result;
        //        var jsonResult = "";
        //        if (response.IsSuccessStatusCode)
        //        {
        //            jsonResult = response.Content.ReadAsStringAsync().Result;
        //            jsonResult = jsonResult.Replace("{\"status\":{\"code\":\"200\",\"desc\":\"OK\"},", "")
        //                .Replace("result\":", "");
        //            jsonResult = jsonResult.Replace("{\"status\":{\"code\":\"404\",\"desc\":\"Not Found\"}}", "")
        //                .Replace("result\":", "");
        //            jsonResult = jsonResult.Replace("\"{", "[{");
        //            jsonResult = jsonResult.Replace("}}", "}]");
        //            dtJson = new DataTable();
        //            dtJson = (DataTable)JsonConvert.DeserializeObject(jsonResult, (typeof(DataTable)));
        //        }
        //    }
        //    catch( Exception ex)
        //    {

        //    }
        //    return Json<DataTable>(dtJson);
        //}
    }
}

            //public class Mobile
            //{
            //    public string Mobile_no { get; set; }
            //}
