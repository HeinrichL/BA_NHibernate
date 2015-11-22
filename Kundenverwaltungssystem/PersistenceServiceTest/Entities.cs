using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace PersistenceServiceTest
{
    [Serializable]
    public abstract class Entity
    {
        public virtual int Version { get; set; }
        public virtual int ID { get; set; }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            foreach(PropertyInfo pi in GetType().GetProperties())
            {
                if (!pi.GetValue(this).Equals(pi.GetValue(obj)))
                    return false;
            }
            return true;
        }
    }
    public class TestClass : Entity
    {
        public TestClass() { }

        public virtual string Bla { get; set; }
        public virtual IList<TestMember> Members { get; set; }
    }

    public class TestMap : ClassMap<TestClass>
    {
        public TestMap()
        {
            Id(x => x.ID);
            Map(x => x.Bla);
            Version(x => x.Version);
            HasMany(x => x.Members).Cascade.All();
        }
    }

    [Serializable]
    public class TestMember : Entity
    {
        public TestMember() { }

        public virtual DateTime Hehe { get; set; }
    }

    public class MemberMap : ClassMap<TestMember>
    {
        public MemberMap()
        {
            Id(x => x.ID);
            Map(x => x.Hehe);
            Version(x => x.Version);
        }
    }
}