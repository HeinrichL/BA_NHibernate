using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace PersistenceService
{
    public static class HibernateSessionFactory
    {
        private const string ConnString = DatabaseConfig.ConnString;

        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeSessionFactory();
                return _sessionFactory;
            }

        }
        private static void InitializeSessionFactory(bool dropCreate = false, bool sqlite = false)
        {
            FluentConfiguration fluentConfig = Fluently.Configure();

            fluentConfig.Database(MsSqlConfiguration.MsSql2012
            .ConnectionString(ConnString).AdoNetBatchSize(50)
            .ShowSql);


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
                                                                   if (dropCreate)
                                                                   {
                                                                       var se = new SchemaExport(cfg);
                                                                       se.Drop(true, true);
                                                                       se.Create(true, true);
                                                                   }

                                                                   else
                                                                       new SchemaUpdate(cfg).Execute(false, true);
                                                               })
                                                      .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
