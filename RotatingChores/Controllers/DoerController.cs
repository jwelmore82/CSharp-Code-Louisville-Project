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
    public class DoerController : AppControllerBase
    {
        // GET: Doer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                NullIdEncountered();
            }
            using (var context = new RotatingChoresContext())
            {
                var doer = GetDoerById(id, context);
                var userGroup = GetUserGroup(context);
                var doerModel = ChoreDoerModel.ConvertFromDoer(doer);
                doerModel.AddChoresList(doer, userGroup);
                return View(doerModel);
            }
        }

        public ActionResult Add()
        {
            
            return View();
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
                catch (Exception e)
                {
                    TempData["FailureMessage"] = e.Message;
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Chores");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                NullIdEncountered();
            }
            using (var context = new RotatingChoresContext())
            {
                var doer = GetDoerById(id, context);
                if (doer != null)
                {
                    var doerModel = ChoreDoerModel.ConvertFromDoer(doer);
                    return View(doerModel);
                }
                
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(ChoreDoerModel doerModel)
        {
            using (var context = new RotatingChoresContext())
            {
                var doer = doerModel.GetRepresentedDoer(context);
                var group = GetUserGroup(context);
                doerModel.AddChoresList(doer, group);
                ValidateChores(doerModel);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Message"] = "Chore doer profile has been updated!";
                    return RedirectToAction("Index");
                }
            }
            return View(doerModel);
        }

        private static ChoreDoer GetDoerById(int? id, RotatingChoresContext context)
        {
            if (id == null)
            {
                return null;
            }
            return context.ChoreDoers.SingleOrDefault(c => c.ChoreDoerId == id);
        }
        private void ValidateChores(ChoreDoerModel model)
        {
            var pass = true;
            foreach (var chore in model.Chores)
            {
                if (chore.Difficulty > model.MaxDifficulty)
                {
                    pass = false;
                }
            }
            if (!pass)
            {
                ModelState.AddModelError("MaxDifficulty", "This user has chores assigned above this difficulty. Reassign chores or raise Max Difficulty");
            }
        }
    }
}
