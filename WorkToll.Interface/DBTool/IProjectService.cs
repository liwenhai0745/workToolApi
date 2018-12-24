using System;
using System.Collections.Generic;
using System.Text;
using WorkTool.Model.DBTool;

namespace WorkTool.Interface.DBTool
{
    public interface IProjectService : IDBService<Project>, IDependency
    {
    }
}
