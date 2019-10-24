using System;
using NHibernate.Proxy;

namespace Logic.Common
{
    public abstract class Entity
    {
        //we don't want client to set the Id
        public virtual long Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetRealType() != other.GetRealType())
                return false;

            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetRealType().ToString() + Id).GetHashCode();
        }

        //This create a dependency in NHibernate
        //try this instead
        
        /*
        private static Type RemoveProxyType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            // ReSharper disable once PossibleNullReferenceException
            if (type.FullName.Contains("DynamicProxies")) return type.BaseType;
            if (type.GetInterfaces().Any(t => t.Name == "INHibernateProxy")) return type.BaseType;

        return type;
    }
        
        */
        private Type GetRealType()
        {
            return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);
        }
    }
}
