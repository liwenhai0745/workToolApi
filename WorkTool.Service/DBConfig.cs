using System;
using System.Collections.Generic;
using System.Text;
using WorkTool.Model.ProgramConfig;

namespace WorkTool.Service
{
    public class DBConfig
    {
        protected static string _DBConString;
        public static void SetDbConString(string _DBCon) {
            _DBConString = _DBCon;
        }

        public static string DBConString { get { return _DBConString; } }

    }
}
