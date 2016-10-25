using Microsoft.AspNet.Identity;
using RotatingChores.Models;
using RotatingChoresData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Principal;
using RotatingChores.Extensions;

namespace RotatingChores.Controllers
{
    public class ChoreController : Controller
    {
        // GET: Chore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var choreContext = new RotatingChoresContext();
            var chore = ChoreModel.ConvertFromChore(choreContext.Chores.Single(c => c.ChoreId == id));
            return View(chore);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var choreContext = new RotatingChoresContext();
            var chore = ChoreModel.ConvertFromChore(choreContext.Chores.Single(c => c.ChoreId == id));
            return View(chore);
        }

        public ActionResult Add()
        {
            var chore = new ChoreModel();
            return View(chore);
        }

        [HttpPost]
        public ActionResult Add(ChoreModel newChore)
        {
            var addingChore = newChore.ConvertToChore();

            addingChore.GroupId = User.Identity.GetGroupId();

            using (var context = new RotatingChoresContext())
            {

                context.Chores.Add(addingChore);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}