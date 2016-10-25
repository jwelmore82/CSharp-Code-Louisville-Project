using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RotatingChoresData;
using RotatingChores.Models;
using System.Data.Entity;
using System.Web.Security;

using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;

namespace RotatingChores.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                
                using (var context = new RotatingChoresContext())
                using (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>())
                {
                    string email = _userManager.GetEmail(id);
                    var choreDoer = context.ChoreDoers.FirstOrDefault(c => c.Email == email);
                    if (choreDoer == null)
                    {
                        ViewBag.Message = "You have not set up your Chore Doer Profile";
                    }
                    else
                    {
                        ViewBag.Message = "Looks like you're in the system!";
                    }
                }

                return View();
            }
            ViewBag.Message = "Sign In to start keeping track of chores!";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}