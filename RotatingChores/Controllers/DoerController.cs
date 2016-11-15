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
            var doerModel = new ChoreDoerModel();
            return View(doerModel);
        }

        [HttpPost]
        public ActionResult Add(ChoreDoerModel model)
        {

            using (var context = new RotatingChoresContext())
            {
                var newDoer = context.ChoreDoers.Create();
                newDoer.GroupId = User.Identity.GetGroupId();
                model.UpdateDoer(newDoer);
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
