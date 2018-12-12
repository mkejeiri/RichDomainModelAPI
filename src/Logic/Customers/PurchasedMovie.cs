using System;
using Logic.Common;
using Logic.Movies;

//using Newtonsoft.Json;

namespace Logic.Customers
{
    //this entity is immutable after creating one we aren't suppose to change anything on it.,
    //it's created by the customer and it's part of the Customer aggregate, so it's internal :
    //client code shouldn't be able to instantiate it
    public class PurchasedMovie : Entity
    {


        //[JsonIgnore
        //DRY: We could assign different id movie for different Movie prop!!!
        //This is a technical concern NOT an ubiquitous languages!!! We could use Movie instead when it's possible
        //public virtual long MovieId { get; set; }
        public virtual Movie Movie { get; protected set; }

        //[JsonIgnore]
        //This is a technical concern NOT an ubiquitous languages!!! We could use customer instead when it's possible
        //public virtual long CustomerId { get; set; }
        public virtual Customer Customer { get; protected set; }

        private decimal _price;
        public virtual Dollars Price
        {
            get => Dollars.Of(_price);
            protected set => _price = value;
        }

        //This is a primitive type we could also aim in changing it (like ExpirationDate) into a ValueObject, but here isn't worth it 
        //because doesn't have any additional logic.
        public virtual DateTime PurchaseDate { get; protected set; }
        //public virtual DateTime? ExpirationDate { get; set; }
        private DateTime? _expirationDate;
        public virtual ExpirationDate ExpirationDate
        {
            get => (ExpirationDate)_expirationDate;
            protected set => _expirationDate = value;
        }

        //public PurchasedMovie(Movie movie, Customer customer, Dollars price, DateTime purchaseDate, DateTime? expirationDate): this()
        //{
        //    Movie = movie ?? throw new ArgumentNullException(nameof(movie));
        //    Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        //    _price = price;
        //    PurchaseDate = purchaseDate;
        //    _expirationDate = expirationDate ?? throw new ArgumentNullException(nameof(expirationDate));
        //}

       
        //protected instead of private so nHibernate is able to instantiate it
        protected PurchasedMovie() { }

        //client code shouldn't be able to instantiate it (see details above)
        internal PurchasedMovie(Movie movie, Customer customer, Dollars price, ExpirationDate expirationDate)
        {
            if (price == null || price.IsZero)
            {
                throw new ArgumentNullException(nameof(price));
            }
            if (expirationDate == null || expirationDate.IsExpired)
            {
                throw new ArgumentNullException(nameof(expirationDate));
            }

            Movie = movie ?? throw new ArgumentNullException(nameof(movie));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            _price = price;
            _expirationDate = expirationDate;
            PurchaseDate = DateTime.UtcNow;
        }
    }
}
