using System;
using FluentNHibernate.Mapping;

namespace Logic.Customers
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Id(x => x.Id);

            //so Hibernate could work with the backing field directly instead of the prop
            Map(x => x.Name).CustomType<string>().Access.CamelCaseField(Prefix.Underscore);
            Map(x => x.Email).CustomType<string>().Access.CamelCaseField(Prefix.Underscore);
            //Map(x => x.Status).CustomType<int>();
            //Map(x => x.StatusExpirationDate).CustomType<DateTime>().Access.CamelCaseField(Prefix.Underscore).Nullable();
            Map(x => x.MoneySpent).CustomType<decimal>().Access.CamelCaseField(Prefix.Underscore);    

            HasMany(x => x.PurchasedMovies).Access.CamelCaseField(Prefix.Underscore);
            Component(x => x.Status, y =>
              {
                                    //Column name
                  y.Map(zx => zx.Type , "Status").CustomType<int>();
                  y.Map(xy => xy.ExpirationDate, "StatusExpirationDate").CustomType<DateTime>()
                            .Access.CamelCaseField(Prefix.Underscore)
                            .Nullable();
              });
        }
    }
}
