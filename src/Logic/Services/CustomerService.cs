using System;
using System.Linq;
using Logic.Entities;

namespace Logic.Services
{
    public class CustomerService
    {
        //private readonly MovieService _movieService;

        //public CustomerService(MovieService movieService)
        //{
        //    _movieService = movieService;
        //}
        //this works primarily by the data from the movie class --> move it to the movie class
        //private Dollars CalculatePrice(CustomerStatus status, LicensingModel licensingModel)
        //{
        //    Dollars price;
        //    switch (licensingModel)
        //    {
        //        case LicensingModel.TwoDays:
        //            price = Dollars.Of(4);
        //            break;

        //        case LicensingModel.LifeLong:
        //            price = Dollars.Of(8);
        //            break;

        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //    //REM: if (status == CustomerStatus.Advanced && (statusExpirationDate == null || statusExpirationDate.Value >= DateTime.UtcNow))
        //    if (status.IsAdvanced/*CustomerStatus.Advanced && !statusExpirationDate.IsExpired*/)
        //    {
        //        price = price * 0.75m;
        //    }

        //    return price;
        //}

        //public void PurchaseMovie(Customer customer, Movie movie)
        //{
        //    //var expirationDate = movie.GetExpirationDate(movie.LicensingModel);
        //    //Dollars price = CalculatePrice(customer.Status , movie.LicensingModel);
        //    var expirationDate = movie.GetExpirationDate();            
        //    Dollars price = movie.CalculatePrice(customer.Status);
        //    customer.AddPurchasedMovie(movie);
        //    //customer.AddPurchasedMovie(movie, expirationDate, price);


        //    //this additional opportunity to mess up: Nothing garanties that we won't assign as different customer to the purchased movie
        //    //to make sure that the client won't use a different customer for the PurchasedMovie!!!! -> the followinf code to the AddPurchasedMovie
        //    //REM: var purchasedMovie = new PurchasedMovie
        //    //REM: /{
        //    //REM:     MovieId = movie.Id,
        //    //REM:     CustomerId = customer.Id,
        //    //REM:     ExpirationDate = expirationDate,
        //    //REM:     Price = price,
        //    //REM:     PurchaseDate = DateTime.UtcNow
        //    //REM: };


        //    //*** moved up: customer.AddPurchasedMovie(movie, expirationDate, price);
        //    //We cannot add movie without increasing the amount of MoneySpent (potential bug): this should go inside AddPurchasedMovie(purchasedMovie)
        //    //REM: customer.MoneySpent += price; 
        //}

        //public bool PromoteCustomer(Customer customer)
        //{
        //    // at least 2 active movies during the last 30 days
        //    if (customer.PurchasedMovies.Count(x => 
        //    x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Value >= DateTime.UtcNow.AddDays(-30)) < 2)
        //        return false;

        //    // at least 100 dollars spent during the last year
        //    if (customer.PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
        //        return false;

        //    customer.Status = customer.Status.Promote();
        //    //customer.StatusExpirationDate = (ExpirationDate)(DateTime.UtcNow.AddYears(1));
        //    return true;
        //}
    }
}
