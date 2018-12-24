using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkTool.Model
{
    [Table("USER")]
    public class User
    {
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
    }
}
