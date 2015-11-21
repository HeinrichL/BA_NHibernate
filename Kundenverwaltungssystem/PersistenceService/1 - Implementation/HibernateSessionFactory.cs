using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace PersistenceService.Implementation
{
    public static class HibernateSessionFactory
    {
        private const string ConnString = @"server=Thinkpad\SQLSERVER2012; Database=KundenverwaltungNHibernate;Integrated Security=SSPI;";
        private const string ConnStringSQLite = "test.tb";

        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
#if DEBUG
                    InitializeSessionFactory(true, true);
#else
                    InitializeSessionFactory();
#endif
                return _sessionFactory;
            }

        }
        private static void InitializeSessionFactory(bool dropCreate = false, bool sqlite = false)
        {
            FluentConfiguration fluentConfig = Fluently.Configure();

            if (sqlite)
                fluentConfig.Database(SQLiteConfiguration.Standard
                    //.UsingFile("test.db")
                    .InMemory()
                    .ShowSql);
            else
            {
                fluentConfig.Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(ConnString)
                    .ShowSql);
            }

            //// Aus Softwareprojekt SS 2014 - PersistenceServicesFactory
            // get all user assemblies
            ICollection<Assembly> allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
                         assembly.ManifestModule.Name != "<In Memory Module>"
                     && !assembly.FullName.StartsWith("mscorlib")
                     && !assembly.FullName.StartsWith("System")
                     && !assembly.FullName.StartsWith("Microsoft")).ToList();
            foreach (Assembly mappingAssembly in allAssemblies)
            {
                // find all types that derive from ClassMap<>
                IList<Type> types = mappingAssembly.GetTypes().Where(t =>
                       t != typeof(AutoMapping<>)
                    && t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<>)).ToList();

                // if there are any, we add their assembly
                if (types.Count > 0)
                {
                    fluentConfig.Mappings(m => m.FluentMappings.AddFromAssembly(mappingAssembly));
                }
            }

            _sessionFactory = fluentConfig.ExposeConfiguration(cfg =>
                                                               {
                                                                   if(sqlite)
                                                                       config = cfg;
                                                                   else
                                                                   {
                                                                       if (dropCreate)
                                                                           new SchemaExport(cfg).Execute(false, true, false);
                                                                       else
                                                                           new SchemaUpdate(cfg).Execute(false, true);
                                                                   }
                                                          
                                                          
                                                      })
                                                      .BuildSessionFactory();
        }

        public static void BuildSchema(ISession session)
        {
            new SchemaExport(config).Execute(true, true, false, session.Connection, null);
        }

        private static Configuration config;

        public static ISession OpenSession()
        {
            var session = SessionFactory.OpenSession();
            BuildSchema(session);
            return session;
        }
    }

    //public class SqlStatementInterceptor : EmptyInterceptor
    //{
    //    public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
    //    {
    //        Trace.WriteLine(sql.ToString());
    //        return sql;
    //    }
    //}
}
