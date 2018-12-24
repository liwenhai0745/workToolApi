using Autofac;
using Autofac.Core;
using System.Data;
using System.Data.SqlClient;
using WorkTool.Interface;
using MySql.Data.MySqlClient;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using WorkTool.Model.ProgramConfig;
using System.IO;
using WorkTool.Service;
using WorkTool.Model;

namespace workToolApi
{
    public class AutofacModule : Module
    {
        private string MySqlConnectionString="";
        public AutofacModule(IConfiguration configuration)
        {
            MySqlConnectionString = configuration.GetSection("WorkTool")["MySqlCon"];
        }

        protected override void Load(ContainerBuilder builder)
        {
           
            builder.Register(c => ConnectionFactory.GetSysConnection(MySqlConnectionString, 1)).As<IDbConnection>().InstancePerLifetimeScope();

            Type baseType = typeof(IDependency);
            // 获取所有相关类库的程序集
            //var assemblys = AppDomain.CurrentDomain.GetAssemblies();

            var binPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var assembly = System.Reflection.Assembly.LoadFrom(Path.Combine(binPath, "WorkTool.Service.dll"));

            builder.RegisterInstance(new BaseReturnDTO()).As<BaseReturnDTO>();

            builder.RegisterAssemblyTypes(assembly)
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces().InstancePerLifetimeScope();//InstancePerLifetimeScope 保证对象生命周期基于请求


        }
    }
}