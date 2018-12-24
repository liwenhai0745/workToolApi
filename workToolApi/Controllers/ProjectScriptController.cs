using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json.Linq;
using WorkTool.Interface;
using WorkTool.Interface.DBTool;
using WorkTool.Model;
using WorkTool.Model.DBTool;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace workToolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectScriptController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private IProjectScriptService service;
        private IApiService apiService;

        public ProjectScriptController(IProjectScriptService _service, IHostingEnvironment hostingEnvironment, IApiService _ApiService)
        {
            _hostingEnvironment = hostingEnvironment;
            this.service = _service;
            apiService = _ApiService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<dynamic> Get()
        {
            var UserID = Convert.ToInt32(User.Claims.First(x => x.Type == "sub").Value);
            var projects = service.GetMyScript(UserID);
            return projects;
        }

        // GET api/<controller>/5 取得一个资源
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>  新增一个没有id的资源
        [HttpPost]
        public BaseDTO Post([FromBody] ProjectScript Data)
        {
            var result = apiService.GetApiResult<BaseDTO>();
            var UserID = apiService.GetUserID(User);

          
            if (Data.ID == 0)
            {
                string[] Names = GetString( Data.Name.Split(','));
                string errMsg = "";
                foreach (var item in Names)
                {
                    var TempData = Data.Clone() as ProjectScript;
                    TempData.CreateTime = DateTime.Now;
                    TempData.CreateUserID = UserID;
                    TempData.Name = item.Trim();

                    bool isValid = true;
                    try
                    {
                        isValid = service.ValidateScript(UserID, TempData);
                    }
                    catch (Exception ex)
                    {
                        isValid = false;
                        errMsg += ex.Message + ";";
                       
                    }

                    if (isValid) {
                        long ID = service.Insert(TempData);
                        TempData.ID = ID;
                    }
                   
                }

                if (!string.IsNullOrEmpty(errMsg)) {
                    apiService.SetApiResult(result, DoResult.Failed, errMsg);
                    return result;
                }

            }
            else
            {
                try
                {
                    service.ValidateScript(UserID, Data);
                }
                catch (Exception ex)
                {
                    apiService.SetApiResult(result, DoResult.Failed, ex.Message);
                    return result;
                }
                service.Update(Data);
            }
            return result;
        }

        //传入的参数是一个有重复元素的数组，返回一个去重之后的新的数组
        private static string[] GetString(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)//遍历数组成员
            {
                if (list.IndexOf(values[i].ToLower()) == -1)//对每个成员做一次新数组查询如果没有相等的则加到新数组
                    list.Add(values[i]);
            }
            return list.ToArray();
        }

        //// PUT api/<controller>/5 更新一个资源。或新增一个含 id 资源(如果 id 不存在)
        //[HttpPut("{id}")]
        //public void Put( int id, [FromBody] JObject Data)
        //{
        //}

        // DELETE api/<controller>/5 删除一个资源
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.Delete(new ProjectScript() { ID = id });
        }


        [HttpPost("Export")]
        public dynamic Export([FromBody] List<int> Data)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var UserID = Convert.ToInt32(User.Claims.First(x => x.Type == "sub").Value);
            string errMsg = "";
            string zipName = "";
            bool result = service.GetScript(webRootPath, UserID, Data, out zipName, out errMsg);
            return new
            {
                result,
                zipName,
                errMsg
            };
        }

        [HttpPost("ExportByProject")]
        public dynamic ExportByProject(int projectId)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var UserID = apiService.GetUserID(User);
            string errMsg = "";
            string zipName = "";
            bool result = service.GetScriptByProjectID(webRootPath, UserID, projectId, out zipName, out errMsg);
            return new
            {
                result,
                zipName,
                errMsg
            };
        }


        [AllowAnonymous]
        [HttpGet("Download")]
        public IActionResult DownLoad(string fileName)
        {
            var addrUrl = _hostingEnvironment.WebRootPath+ "\\SQL\\" + fileName;
            var stream = System.IO.File.OpenRead(addrUrl);
            string fileExt = ".zip";
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi, Path.GetFileName(addrUrl));

        }


    }
}
