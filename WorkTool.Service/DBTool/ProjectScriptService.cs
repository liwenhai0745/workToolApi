using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WorkTool.Interface.DBTool;
using WorkTool.Model.DBTool;
using Dapper;
using System.Linq;
using System.IO;
using WorkTool.Service.CommonTool;

namespace WorkTool.Service.DBTool
{
    public class ProjectScriptService : DBService<ProjectScript>, IProjectScriptService
    {
        public ProjectScriptService(IDbConnection _dbConnection) : base(_dbConnection)
        {
        }

        public IEnumerable<dynamic> GetMyScript(int UID)
        {
            string sql = @"SELECT case t1.OPType when 1 then '添加' else '修改' end  OPText,             case t1.OBType when 1 then '存储过程' when 2 then '表' else '视图' end  DBText,
            t1.*,t2.USERNAME,t3.`Name` ProjectName	 from ProjectScript t1 
            JOIN `USER` t2 on t1.CreateUserID=t2.ID 
            JOIN Project t3 on t1.ProjectID=t3.ID 
            where t1.CreateUserID=@UserID";
            var result = Query(sql, new { UserID = UID });
            return result;
        }

        public bool GetScript(string webRootPath, int UID,List<int> ids,out string zipName, out string errMsg)
        {
            errMsg = "";
            StringBuilder Sb = new StringBuilder($"--{DateTime.Now.ToString()}通过小工具生成\n");
            int i = 1;
            foreach (var item in ids)
            {
                string tmpErrmsg = "";
                Sb.Append(GetScriptString("NO." + i, UID, item, out tmpErrmsg));
                i++;
            }
            zipName = BuildScripZIP(webRootPath, UID, Sb.ToString());
            return true;
        }

        public bool GetScriptByProjectID(string webRootPath, int UID, int ProjectID, out string zipName, out string errMsg)
        {
            errMsg = "";
            var ids = dbCon.Query<int>("SELECT ID FROM `ProjectScript` where ProjectID=@ProjectID", new { ProjectID }).ToList<int>();
            GetScript(webRootPath, UID, ids, out zipName, out errMsg);
            return true;
        }

        private static string BuildScripZIP(string webRootPath, int UID, string scriptContent)
        {
            string zipName;
            string FileName = $"{UID}-{DateTime.Now.ToString("yyyyMMddHHmmss")}.sql";

            string FilePath = webRootPath + "\\SQL\\" + FileName;
            zipName = FileName.Replace(".sql", ".zip");
            string FileZipPath = webRootPath + "\\SQL\\" + zipName;
            File.AppendAllText(FilePath, scriptContent);
            var zipTool = new ZipUtils();
            string zipMsg = "";
            bool result = zipTool.ZipFiles(new string[] { FilePath }, FileZipPath, out zipMsg);
            if (result)
            {
                File.Delete(FilePath);
            }

            return zipName;
        }

        private IDbConnection GetSqlConnection(int projectid,string DBName) {
            var project = new ProjectService(dbCon).Get(projectid);
            string sqlCon = $"Data Source={project.DBIP};Initial Catalog={DBName};User ID={project.DBUserName};Password={project.DBPWD}";
            //查询SQL数据库
            IDbConnection Con = new SqlConnection(sqlCon);
            return Con;
        }

        private string GetScriptString(string ScriptNo, int UID,int id, out string errMsg) {
            errMsg = "";
            string sql = @"SELECT case t1.OPType when 1 then '添加' else '修改' end  OPText,             case t1.OBType when 1 then '存储过程' when 2 then '表' else '视图' end  DBText,DBIP,DBUserName,DBPWD,
            t1.*,t2.USERNAME,t3.`Name` ProjectName	 from ProjectScript t1 
            JOIN `USER` t2 on t1.CreateUserID=t2.ID 
            JOIN Project t3 on t1.ProjectID=t3.ID 
            where  t1.ID=@ID";
            var results = Query(sql, new {ID=id });
            var result = results.First();
            var User = new UserService(this.dbCon).Get(UID);
            string ProcName = result.Name;
            string DBName = result.DBName;
            int OPType = result.OPType;
            int OBType = result.OBType;
            int ProjectID = result.ProjectID;
            string DBIP = result.DBIP;
            string DBUserName = result.DBUserName;
            string DBPWD = result.DBPWD;

            //查询SQL数据库
            IDbConnection Con = GetSqlConnection(ProjectID,DBName);
            string SqlResult = "";
            switch (OBType)
            {
                case 1:
                case 2:
                    var listResult = Con.Query<string>("sp_helptext", new { objname = ProcName }, commandType: CommandType.StoredProcedure);
                    string Script = string.Join("", listResult.ToArray());
                    if (OPType == 2) {
                        Script = Script.Replace("CREATE PROC", "ALTER PROC");
                    }
                    SqlResult = (OPType==1? ProcExistString(ProcName,OBType):"")+ Script;
                    break;
                default:
                    break;
            }
           
           
            return $"USE [{DBName}] \nGO\n---{ScriptNo} 修改人 {User.USERNAME}\n{SqlResult}\nGO\n\n";
        }

        private string ProcExistString(string procName,int OBType) {
            string script = "";
            if (OBType == 1)
            {
                script = string.Format(@"IF EXISTS
(
    SELECT *
    FROM sys.procedures
    WHERE object_id = OBJECT_ID(N'[dbo].[{0}]')
)
    DROP PROC {0};
GO", procName);
            }
            else if (OBType == 2) {
                script = string.Format(@"IF EXISTS
(
    SELECT *
    FROM sys.views
    WHERE object_id = OBJECT_ID(N'[dbo].[{0}]')
)
    DROP VIEW {0};
GO", procName);

            }
            return "\n"+script+"\n";
        }


        /// <summary>
        /// 验证是否可以添加
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ValidateScript(int UID, ProjectScript Data) {
            bool result = true;

            //1.判断对象是否存在 

           

            string sql = "";
            switch (Data.OBType)
            {
                case 1:
                    sql = $"SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'[dbo].[{Data.Name}]')";
                    break;
                case 2:
                    sql = $"SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[{Data.Name}]')";
                    break;
                case 3:
                    sql = $"SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[{Data.Name}]')";
                    break;
                default:
                    break;
            }
            //查询SQL数据库
            var isExist = AutoSetDBName(Data, sql);
            //IDbConnection Con = GetSqlConnection(Data.ProjectID, Data.DBName);
            //var qResult = Con.Query(sql).SingleOrDefault();
            //bool isExist = true;
            if (!isExist)
            {
                throw new Exception($"对象{Data.Name}不存在！");
            }




            //2.判断是否其它用户已经添加
            var list = dbCon.Query<ProjectScript>("SELECT * FROM ProjectScript where `Name`=@Name and ProjectID=@ProjectID ", new { Name=Data.Name, ProjectID=Data.ProjectID});
            if (list.Count() > 0) {
                var uid = list.First().CreateUserID;
                var UName = new UserService(dbCon).Get(uid).USERNAME;
                throw new Exception($"对象{Data.Name}已经被ID为[{UName}]用户创建！");

            }
            return result;
        }

        private bool AutoSetDBName(ProjectScript Data,string Sql) {
            var dbs = DBInfo.GetDBs();
            dbs.Remove(Data.DBName);
            dbs.Insert(0, Data.DBName);
            //查询SQL数据库
            bool isExist = false;
            foreach (string item in dbs)
            {
                IDbConnection Con = GetSqlConnection(Data.ProjectID, item);
                var qResult = Con.Query(Sql).SingleOrDefault();
                if (qResult != null)
                {
                    //throw new Exception($"对象{Data.Name}不存在！");
                    isExist=true;
                    Data.DBName = item;
                    break;
                }
            }
            return isExist;
        }
    }
}
