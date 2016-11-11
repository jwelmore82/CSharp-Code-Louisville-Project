using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RotatingChores.Models;
using RotatingChoresData;
using RotatingChores.Extensions;

namespace RotatingChores.Controllers
{
    [Authorize]
    public class DoerController : Controller
    {
        // GET: Doer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ChoreDoerModel model)
        {
            var newDoer = model.CovertToDoer();
            newDoer.GroupId = User.Identity.GetGroupId();
            using (var context = new RotatingChoresContext())
            {

                try
                {
                    context.ChoreDoers.Add(newDoer);
                    context.SaveChanges();
                    TempData["Message"] = "New group member added!";
                }
                catch (Exception)
                {

                    return View(model);
                }
            }
            return RedirectToAction("Index", "Chores");
        }
    }
}
