using Logic.Customers;
using Logic.Movies;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Logic.Services;

namespace Api.Utils
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(new SessionFactory(Configuration["ConnectionString"]));
            services.AddScoped<UnitOfWork>();
            services.AddTransient<MovieRepository>();
            services.AddTransient<CustomerRepository>();
            //services.AddTransient<MovieService>();
            //services.AddTransient<CustomerService>();
        }

        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //hosting params removed since no need to check for the ENV
        public void Configure(IApplicationBuilder app)
        {
            //this middleware registration should go first in this method. because we want the exceptionHandler to reside on the top of the excecution stack.
            app.UseMiddleware<ExceptionHandler>();

            //removed as all exception are handled by the middleware
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseMvc();
        }
    }
}
