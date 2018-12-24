using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WorkTool.Interface;
using WorkTool.Interface.DBTool;
using WorkTool.Model;
using WorkTool.Model.DBTool;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace workToolApi.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private IProjectService service;
        private IApiService apiService;
        public ProjectController(IProjectService _service, IApiService _ApiService)
        {
            this.service = _service;
            this.apiService = _ApiService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Project> Get()
        {
            var projects = service.GetAll();
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
        public BaseReturnDTO Post([FromBody] Project Data)
        {
            var result = apiService.GetApiResult<BaseReturnDTO>();
            var UserID = apiService.GetUserID(User);
            if (Data.ID == 0)
            {
                Data.CreateTime = DateTime.Now;
                Data.CreateUserID = UserID;
                long ID = service.Insert(Data);
                Data.ID = ID;
            }
            else {

                var tempProject = service.Get(Convert.ToInt32(Data.ID));
                result = apiService.ValiateIsSelf<BaseReturnDTO>(User, tempProject.CreateUserID);
                if (result.Code == DoResult.Failed) { return result; }

                service.Update(Data);
            }
            result.Data = Data;
            return result;
        }

        //// PUT api/<controller>/5 更新一个资源。或新增一个含 id 资源(如果 id 不存在)
        //[HttpPut("{id}")]
        //public void Put( int id, [FromBody] JObject Data)
        //{
        //}

        // DELETE api/<controller>/5 删除一个资源
        [HttpDelete("{id}")]
        public BaseDTO Delete(int id)
        {
            var tempProject = service.Get(Convert.ToInt32(id));
            var result = apiService.ValiateIsSelf<BaseDTO>(User, tempProject.CreateUserID);
            if(result.Code== DoResult.Failed) { return result; }
            service.Delete(new Project() { ID = id });
            return result;
        }
    }
}
