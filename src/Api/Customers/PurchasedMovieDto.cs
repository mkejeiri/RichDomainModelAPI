using System;
using Api.Movies;

namespace Api.Customers
{
    public class PurchasedMovieDto
    {
        //public  long CustomerId { get; set; }
        public MovieDto MovieDto { get; set; }        
        public  decimal Price { get; set; }
        public  DateTime PurchaseDate { get; set; }
        public  DateTime? ExpirationDate { get; set; }
    }
}
