using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WorkTool.Interface
{
    public interface IDBService<T> where T:class
    {
        IDbConnection GetDbConnection();

        T Get(int id);
        IEnumerable<T> GetAll();
        long Insert(T obj);
        long Insert(IEnumerable<T> list);
        bool Update(T obj);
        bool Update(IEnumerable<T> list);
        bool Delete(T obj);
        bool Delete(IEnumerable<T> list);
        bool DeleteAll<T>();

        IEnumerable<dynamic> Query(string sql, object param = null);

        dynamic QuerySingle(string sql, object param = null);
    }
}
