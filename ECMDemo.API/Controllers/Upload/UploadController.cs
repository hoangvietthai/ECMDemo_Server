using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ECMDemo.API.Controllers
{
    [RoutePrefix("api/v1/ecmdemo")]
    public class UploadController : ApiController
    {
        [HttpPost]
        [Route("upload")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> UploadFile(string folderPath = "")
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> resultMap = new Dictionary<string, object>();
            dict.Add("status", false);
            try
            {
                var physicalStoreConfig = System.Configuration.ConfigurationManager.AppSettings["DocumentFolder"]; ;
                if (physicalStoreConfig.StartsWith("~")) // relative path
                    physicalStoreConfig = System.Web.HttpContext.Current.Server.MapPath(physicalStoreConfig);
                if (!physicalStoreConfig.EndsWith(@"\"))
                    physicalStoreConfig = physicalStoreConfig + @"\";
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 10; //Size = 10 MB  

                        //IList<string> AllowedFileExtensions = new List<string> { ".mp3" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        //var extension = ext.ToLower();
                        //if (!AllowedFileExtensions.Contains(extension))
                        //{
                        //    var message = string.Format("Please upload audio of type .mp3.");

                        //    dict.Add("message", message);
                        //    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        //}
                        if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Tệp quá lớn! Vui lòng chọn tệp dưới 10mb");

                            dict.Add("message", message);
                            return Request.CreateResponse(HttpStatusCode.OK, dict);
                        }
                        else
                        {
                            string path = physicalStoreConfig + folderPath.TrimEnd('/');
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string fullnameFile = folderPath.TrimEnd('/') + "/" + Guid.NewGuid() + ext;
                            var filePath = physicalStoreConfig + fullnameFile;
                            await Task.Run(new Action(() => postedFile.SaveAs(filePath)));
                            resultMap.Add(file, fullnameFile);
                        }
                    }
                }
                if (httpRequest.Files.Count > 0)
                {
                    dict.Add("message", "Upload success!");
                    dict.Add("result", resultMap.ToArray());
                    dict["status"] = true;
                    return Request.CreateResponse(HttpStatusCode.OK, dict);
                }
                else
                {
                    var res = string.Format("Something is not right!");
                    dict.Add("message", res);
                    return Request.CreateResponse(HttpStatusCode.OK, dict);
                }
            }
            catch (Exception ex)
            {
                var res = string.Format(ex.Message);
                dict.Add("message", res);
                return Request.CreateResponse(HttpStatusCode.OK, dict);
            }
        }

        [HttpDelete]
        [Route("upload/remove")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Delete(string path)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                string physicalStoreConfig = string.Empty;
                physicalStoreConfig = System.Configuration.ConfigurationManager.AppSettings["DocumentFolder"];
                if (physicalStoreConfig.StartsWith("~")) // relative path
                    physicalStoreConfig = System.Web.HttpContext.Current.Server.MapPath(physicalStoreConfig);
                if (!physicalStoreConfig.EndsWith(@"\"))
                    physicalStoreConfig = physicalStoreConfig + @"\";
                if (path == null)
                {
                    dict.Add("message", "Path not found.");
                    dict.Add("result", false);
                    var listfolder = Directory.GetDirectories(physicalStoreConfig);
                    return Ok(dict);
                }
                else
                {
                    Directory.Delete(physicalStoreConfig + path, true);
                    dict.Add("message", "Delete success.");
                    dict.Add("result", true);
                    return Ok(dict);
                }
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
                dict.Add("result", false);
                return Content(HttpStatusCode.BadRequest, dict);
            }
        }
    }
}
