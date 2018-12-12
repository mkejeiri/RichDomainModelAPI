using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Common;
using Logic.Movies;
//using System.ComponentModel.DataAnnotations;

//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

namespace Logic.Customers
{
    public class Customer : Entity
    {
        //[Required]
        //[MaxLength(100, ErrorMessage = "Name is too long")]
        //public virtual CustomerName  Name { get; set; }

        private string _name;

        //make a proxy on top of _name prop
        public virtual CustomerName Name
        {
            //get => CustomerName.Create(_name).Value;
            //set =>  _name = value.Value;
            //After implicit & explicit conversion
            get => (CustomerName)_name;
            set => _name = value;
        }

        //[Required]
        //[RegularExpression(@"^(.+)@(.+)$", ErrorMessage = "Email is invalid")]
        //public virtual Email Email { get; set; }
        //private string _email;
        private readonly string _email;
        public virtual Email Email
        {
            //get => Email.Create(_email).Value;
            //set => _email = value.Value;
            //After implicit & explicit conversion
            get => (Email)_email;
            //Never got to change, we never change the customer email once it created!
            //protected set => _email = value;
        }

        //protected: client code never modifies it!
        public virtual CustomerStatus Status { get; protected set; }

        ////[JsonConverter(typeof(StringEnumConverter))]
        //public virtual CustomerStatus Status { get; set; }

        ////public virtual DateTime? StatusExpirationDate { get; set; }
        //private DateTime? _statusExpirationDate { get; set; }
        //public ExpirationDate StatusExpirationDate
        //{
        //    get => (ExpirationDate)_statusExpirationDate;
        //    set => _statusExpirationDate = value;
        //}


        //public virtual decimal MoneySpent { get; set; }
        private decimal _moneySpent;

        public virtual Dollars MoneySpent
        {
            get => Dollars.Of(_moneySpent);
            protected set => _moneySpent = value;
        }

        

        //this will allow client to do the clearing of the list or the set to null, those are illegal operations from Domain point of view
        //public virtual IList<PurchasedMovie> PurchasedMovies { get; set; }        
        private IList<PurchasedMovie> _purchasedMovies;
        public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

        public Customer(CustomerName name, Email email) : this()
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _email = email ?? throw new ArgumentNullException(nameof(email));
            MoneySpent = Dollars.Of(0);
            Status = CustomerStatus.Regular;
            //Status = (CustomerStatus)CustomerStatusType.Regular;
            //StatusExpirationDate = ExpirationDate.Infinite;
        }
        protected Customer()
        {
            _purchasedMovies = new List<PurchasedMovie>();
        }

        public virtual Result CanPromote()
        {
            if (Status.IsAdvanced)
            {
                return Result.Fail("The customer already has the Advanced status");
            }
            if (PurchasedMovies.Count(x =>
            x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Value >= DateTime.UtcNow.AddDays(-30)) < 2)
                return Result.Fail("Customer should at least have 2 active movies during the last 30 days");

            
            if (PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
                return Result.Fail("Customer should at least 100 dollars spent during the last year");
          
            return Result.Ok();
        }


        public virtual void Promote()
        {
            if (CanPromote().IsFailure)
            {
                throw new Exception();
            }
            Status = Status.Promote();
            //customer.StatusExpirationDate = (ExpirationDate)(DateTime.UtcNow.AddYears(1));            
        }
               
        public virtual void PurchaseMovie(Movie movie)
        {
            if (HasPurchasedMovie(movie))
            {
                throw new Exception();
            }
            var expirationDate = movie.GetExpirationDate();
            Dollars price = movie.CalculatePrice(Status);
            var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
            _purchasedMovies.Add(purchasedMovie);
            _moneySpent += price;
        }

        public bool HasPurchasedMovie(Movie movie)
        {
            return PurchasedMovies.Any(x => x.Movie == movie && !x.ExpirationDate.IsExpired);
        }
        
    }
}
