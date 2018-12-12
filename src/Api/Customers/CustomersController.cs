using System.Collections.Generic;
using System.Linq;
using Api.Movies;
using Api.Utils;
using Logic.Common;
using Logic.Customers;
using Logic.Movies;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
//using Logic.Services;

namespace Api.Customers
{
    [Route("api/[controller]")]
    public class CustomersController : BaseController
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;       

        //private readonly CustomerService _customerService;

        public CustomersController(UnitOfWork unitOfWork,MovieRepository movieRepository, CustomerRepository customerRepository/*, CustomerService customerService*/) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
            //_customerService = customerService;
        }       

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null) return NotFound();

            var dto = new CustomerDto
            {
                Id = customer.Id, 
                Email = customer.Email.Value ,
                Name = customer.Name.Value ,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.ToString(),
                StatusExpirationDate = customer.Status.ExpirationDate,
                PurchasedMoviesDtos = customer.PurchasedMovies.Select(x =>
                    new PurchasedMovieDto
                    {
                        MovieDto = new MovieDto
                        {
                            Id = x.Movie.Id,
                            Name = x.Movie.Name
                        },
                        Price = x.Price,
                        PurchaseDate = x.PurchaseDate,
                        ExpirationDate = x.ExpirationDate
                    }).ToList()
            };

            //return Json(dto);
            return Ok(dto);
        }

        [HttpGet]
        //public JsonResult GetList()
        public IActionResult GetList()
        {
            IReadOnlyList<Customer> customers = _customerRepository.GetList();
            List<CustomerInListDto>  dtos= customers.Select(x => new CustomerInListDto
            {
                Id = x.Id,
                Name = x.Name.Value,
                Email = x.Email.Value,
                Status = x.Status.ToString(),
                StatusExpirationDate = x.Status.ExpirationDate,
                MoneySpent = x.MoneySpent
            }).ToList();
            return Ok(dtos);
            //return Json(customers.Select(x => new CustomerInListDto
            //{
            //    Id= x.Id,
            //    Name=x.Name.Value ,
            //    Email= x.Email.Value,
            //    Status = x.Status.ToString() ,
            //    StatusExpirationDate = x.Status.ExpirationDate,
            //    MoneySpent = x.MoneySpent 
            //}).ToList()); 
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto item)
        {
            //No need for try catch stm since they are managed by the middleware.
            //try
            //{
                Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
                Result<Email> emailOrError = Email.Create(item.Email);
                Result result = Result.Combine(customerNameOrError, emailOrError);

                if (result.IsFailure)
                {
                    //return BadRequest(result.Error);
                    return Error(result.Error);
                }

                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                if (_customerRepository.GetByEmail(emailOrError.Value) != null)
                {
                    //return BadRequest("Email is already in use: " + item.Email);
                    return Error("Email is already in use: " + item.Email);
                }

                _customerRepository.Add(new Customer(customerNameOrError.Value, emailOrError.Value));
                //_customerRepository.SaveChanges();
                return Ok();
            //}
            //catch (Exception e)
            //{
            //    return StatusCode(500, new { error = e.Message });
            //}
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
        {
            //try
            //{
                Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
                Result result = Result.Combine(customerNameOrError);

                if (result.IsFailure)
                {
                    //return BadRequest(result.Error);
                    return Error(result.Error);
                }
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                                

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    //return BadRequest("Invalid customer id: " + id);
                    return Error("Invalid customer id: " + id);
                }

                customer.Name = customerNameOrError.Value;
                //_customerRepository.SaveChanges();

                return Ok();
            //}
            //catch (Exception e)
            //{
            //    return StatusCode(500, new { error = e.Message });
            //}
        }

        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            //try
            //{
                Movie movie = _movieRepository.GetById(movieId);
                if (movie == null)
                {
                    //return BadRequest("Invalid movie id: " + movieId);
                    return Error("Invalid movie id: " + movieId);
                }

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    //return BadRequest("Invalid customer id: " + id);
                    return Error("Invalid customer id: " + id);
                }

            /*
                      //if (customer.PurchasedMovies.Any(x => x.Movie  == movie  && !x.ExpirationDate.IsExpired)):
                       Clear responsability of the customer, the client shouldn't have the knowledge about the movies
                       purchased by the customer.add HasPurchasedMovie to the customer.
                 
           */
            if (customer.HasPurchasedMovie(movie))
            {
                    //return BadRequest("The movie is already purchased: " + movie.Name);
                    return Error("The movie is already purchased: " + movie.Name);
            }

            //_customerService.PurchaseMovie(customer, movie);
            /*
             * we could still purchase an already purchased  movie. it's loophole that we need to close inside PurchaseMovie method
             */
            customer.PurchaseMovie(movie);

                //_customerRepository.SaveChanges();

                return Ok();
            //}
            //catch (Exception e)
            //{
            //    return StatusCode(500, new { error = e.Message });
            //}
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            //try
            //{
                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    //return BadRequest("Invalid customer id: " + id);
                    return Error("Invalid customer id: " + id);
                }

                //if (customer.Status.IsAdvanced) 
                //{
                //    //return BadRequest("The customer already has the Advanced status");
                //    return Error("The customer already has the Advanced status");
                //}

                //bool success = _customerService.PromoteCustomer(customer);
                Result result = customer.CanPromote();
                if (result.IsFailure)
                {
                   return Error(result.Error);
                }
            customer.Promote();
            //_customerRepository.SaveChanges();

            return Ok();
            //}
            //catch (Exception e)
            //{
            //    return StatusCode(500, new { error = e.Message });
            //}
        }
    }
}
