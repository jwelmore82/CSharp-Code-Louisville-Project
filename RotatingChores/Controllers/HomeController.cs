using System.Linq;
using System.Web;
using System.Web.Mvc;
using RotatingChoresData;
using RotatingChores.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace RotatingChores.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                
                using (var context = new RotatingChoresContext())
                using (var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>())
                {
                    string email = userManager.GetEmail(id);
                    var choreDoer = context.ChoreDoers.FirstOrDefault(c => c.Email == email);
                    if (choreDoer == null)
                    {
                        var model = new ChoreDoerModel();
                        model.Email = email;
                        TempData["Message"] = "You have not set up your Chore Doer Profile. Select Max Difficulty 'None' to be excluded from chores.";
                        return RedirectToAction("Add", "Doer", model);                     
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