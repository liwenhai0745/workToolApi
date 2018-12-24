using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WorkTool.Interface.DBTool;
using WorkTool.Model.DBTool;

namespace WorkTool.Service.DBTool
{
    public class ProjectService : DBService<Project>, IProjectService
    {
        public ProjectService(IDbConnection _dbConnection) : base(_dbConnection)
        {
        }
    }
}
