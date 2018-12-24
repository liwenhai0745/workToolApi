using System;
using System.Collections.Generic;
using System.Text;
using WorkTool.Model.DBTool;

namespace WorkTool.Interface.DBTool
{
    public interface IProjectScriptService : IDBService<ProjectScript>, IDependency
    {
        IEnumerable<dynamic> GetMyScript(int UID);

        bool GetScript(string webRootPath, int UID, List<int> ids, out string zipName, out string errMsg);
        bool GetScriptByProjectID(string webRootPath, int UID, int ProjectID, out string zipName, out string errMsg);
        bool ValidateScript(int UID, ProjectScript Data);
    }
}
