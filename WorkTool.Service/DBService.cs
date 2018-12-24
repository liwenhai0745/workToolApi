using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data;
using Dapper;
using WorkTool.Interface;

namespace WorkTool.Service
{
    public class DBService<T> : IDBService<T> where T:class
    {
        protected IDbConnection dbCon { get; }
        public DBService(IDbConnection _dbConnection) {
            this.dbCon = _dbConnection;
        }
        public IDbConnection GetDbConnection()
        {
            return dbCon;
        }

        public T Get(int id)
        {
            return dbCon.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return dbCon.GetAll<T>();
        }

        public long Insert(T obj)
        {
            return dbCon.Insert<T>(obj);
        }

        public long Insert(IEnumerable<T> list)
        {
            return dbCon.Insert<IEnumerable<T>>(list);
        }

        public bool Update(T obj)
        {
            return dbCon.Update<T>(obj);
        }

        public bool Update(IEnumerable<T> list)
        {
            return dbCon.Update(list);
        }

        public bool Delete(T obj)
        {
            return dbCon.Delete(obj);
        }

        public bool Delete(IEnumerable<T> list)
        {
            return dbCon.Delete(list);
        }

        public bool DeleteAll<T1>()
        {
            return dbCon.DeleteAll<T>();
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string sql, object param = null) {
            return dbCon.Query(sql, param);
        }


        public dynamic QuerySingle(string sql, object param = null)
        {
            return dbCon.QuerySingle(sql, param);
        }
    }
}
