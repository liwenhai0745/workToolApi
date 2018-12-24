using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkTool.Model.DBTool
{
    [Table("ProjectScript")]

    public class ProjectScript:ICloneable
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 操作类型1 新增  2编辑
        /// </summary>
        public int OPType { get; set; }
        /// <summary>
        /// 对象类型 1 存储过程  2 视图 3 表
        /// </summary>
        public int OBType { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DBName { get; set; }
        /// <summary>
        /// 项目编号 
        /// </summary>
        public int ProjectID { get; set; }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
