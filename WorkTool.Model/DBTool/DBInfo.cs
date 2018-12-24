using System;
using System.Collections.Generic;
using System.Text;

namespace WorkTool.Model.DBTool
{
    public class DBInfo
    {
        public static List<string> GetDBs() {
            List<string> result = new List<string>();
            result.Add("ztb_shopdict");
            result.Add("ztb_shopinfo");
            result.Add("ztb_shopinfo_snap");
            result.Add("ztb_admin");
            result.Add("ztb_shoptask");
            result.Add("ztb_shoplog");
            result.Add("ztb_shopstat");
            result.Add("ztb_ShopUCard");
            result.Add("ZTB_WX");
            return result;
        }

    }
}
