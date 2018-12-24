using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkTool.Model.DBTool;

namespace workToolApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [HttpGet]
        public List<string> GetDBS() {
            return DBInfo.GetDBs();
        }
    }
}