using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WorkTool.Service
{
    public class ConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="type">1 mysql 2 sqlserver</param>
        /// <returns></returns>
        public static IDbConnection GetSysConnection(string conn,int type ) {
            switch (type)
            {
                case 2:
                    return new SqlConnection(conn);
                default:
                    return new MySqlConnection(conn);
            }
            
        }
    }

}
