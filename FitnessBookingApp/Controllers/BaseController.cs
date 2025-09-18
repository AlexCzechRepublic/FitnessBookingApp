using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FitnessBookingApp.Services;

namespace FitnessBookingApp.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DataService _dataService;

        public BaseController(DataService dataService)
        {
            _dataService = dataService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(username))
            {
                var user = _dataService.GetUserByName(username); // nebo GetUserByUsername
                ViewBag.UserBalance = user != null ? _dataService.GetBalance(user.Id) : 0;
            }
            base.OnActionExecuting(context);
        }
    }
}