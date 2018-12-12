//using Newtonsoft.Json;

using System;
using Logic.Common;
using Logic.Customers;

namespace Logic.Movies
{
    public abstract class Movie : Entity
    {
        public virtual string Name { get; protected set; }

        //[JsonIgnore]
        protected virtual LicensingModel LicensingModel { get; set; }
        public abstract ExpirationDate GetExpirationDate();
        //{
        //    //ExpirationDate result;

        //    switch (LicensingModel)
        //    {
        //        case LicensingModel.TwoDays:
        //            return (ExpirationDate)DateTime.UtcNow.AddDays(2);
                   

        //        case LicensingModel.LifeLong:
        //            return ExpirationDate.Infinite;
                  

        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        public virtual Dollars CalculatePrice(CustomerStatus status)
        {
            //Dollars price;
            decimal discount = status.GetDiscount();
            return GetBasePrice() * (1m - discount);
            //switch (LicensingModel)
            //{
            //    case LicensingModel.TwoDays:
            //        return Dollars.Of(4) * (1m - discount);

            //    case LicensingModel.LifeLong:
            //        return Dollars.Of(8) * (1m - discount);

            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
            //REM: if (status == CustomerStatus.Advanced && (statusExpirationDate == null || statusExpirationDate.Value >= DateTime.UtcNow))
            //it is not the responsability of the movie class to define the discount, it's up to CustomerStatus
            //if (status.IsAdvanced/*CustomerStatus.Advanced && !statusExpirationDate.IsExpired*/)
            //{
            //    price = price * 0.75m;
            //}

            //return price;
        }

        protected abstract Dollars GetBasePrice();
    }

    public class TwoDaysMovie : Movie
    {
        protected override Dollars GetBasePrice()
        {
            return Dollars.Of(4);
        }

        public override ExpirationDate GetExpirationDate()
        {
            return (ExpirationDate)DateTime.UtcNow.AddDays(2);
        }
    }

    public class LifeLongMovie : Movie
    {
        protected override Dollars GetBasePrice()
        {
            return Dollars.Of(8);
        }

        public override ExpirationDate GetExpirationDate()
        {
            return ExpirationDate.Infinite;
        }
    }



}
