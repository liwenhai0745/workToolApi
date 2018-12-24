using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkTool.Model.DBTool
{
    [Table("Project")]

    public class Project
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public string DBIP { get; set; }
        public string DBUserName { get; set; }
        public string DBPWD { get; set; }

    }
}
