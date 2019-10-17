using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Utils
{
    public class BaseController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;// ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        
        protected new IActionResult Ok()
        {
            unitOfWork.Commit();
           return base.Ok(Envelope.Ok());
        }


        protected IActionResult CreatedAt<T>(object obj, T result)
        {
            unitOfWork.Commit();
            return base.Ok(Envelope.Ok(result));
        }


        protected IActionResult Ok<T>(T result)
        {
            unitOfWork.Commit();
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {           
            return base.BadRequest(Envelope.Error (errorMessage));
        }
    }
}
