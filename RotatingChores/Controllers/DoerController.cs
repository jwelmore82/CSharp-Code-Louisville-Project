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
            using (var context = new RotatingChoresContext())
            {
                var group = GetUserGroup(context);
                var modelList = new List<ChoreDoerModel>();
                if (group.Members.Count > 0)
                {
                    foreach (var doer in group.Members)
                    {
                        var doerModel = ChoreDoerModel.ConvertFromDoer(doer);
                        doerModel.AddChoresList(doer, group);
                        modelList.Add(doerModel);
                    }
                }
                return View(modelList);
            }
            
        }

        public ActionResult Details(int? id)
        {
            return ViewDoerById(id);
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
            return ViewDoerById(id);
        }

        [HttpPost]
        public ActionResult Edit(ChoreDoerModel doerModel)
        {
            using (var context = new RotatingChoresContext())
            {
                var doer = doerModel.GetRepresentedDoer(context);
                if (doer != null)
                {
                    var group = GetUserGroup(context);
                    doerModel.AddChoresList(doer, group);
                    ValidateChores(doerModel);
                    if (ModelState.IsValid && IsGroupObject(doer.GroupId))
                    {
                        doerModel.UpdateDoer(doer);
                        context.SaveChanges();
                        TempData["Message"] = "Chore doer profile has been updated!";
                        return RedirectToAction("Index");
                    }
                    return View(doerModel);
                }
                return DoerNotFound();
            }
            
        }

        public ActionResult Delete(int? id)
        {
            return ViewDoerById(id);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int? id)
        {
            using (var context = new RotatingChoresContext())
            {
                var doer = GetDoerById(id, context);
                if (doer != null)
                {
                    if (IsGroupObject(doer.GroupId))
                    {
                        try
                        {
                            context.ChoreDoers.Remove(doer);
                            context.SaveChanges();
                            TempData["Message"] = "Chore doer successfully deleted!";
                        }
                        catch(System.Data.Entity.Infrastructure.DbUpdateException)
                        {
                            TempData["FailureMessage"] = "Member must not have chores assigned or be the last to complete a chore.";
                            return RedirectToAction("Index", "Chores");
                        }
                    }
                    return InvalidGroup();
                }
                return DoerNotFound();
            }
        }

        //Methods for this controller
        //************************************************************

        //Returns a view with ChoreDoerModel if all checks pass
        private ActionResult ViewDoerById(int? id)
        {
            //Check for Id
            if (id == null)
            {
                NullIdEncountered();
            }
            using (var context = new RotatingChoresContext())
            {
                var doer = GetDoerById(id, context);
                //Check for Doer in the database
                if (doer != null)
                {
                    //Check that ChoreDoer is part of user's group
                    if (IsGroupObject(doer.GroupId))
                    {
                        var userGroup = GetUserGroup(context);
                        var doerModel = ChoreDoerModel.ConvertFromDoer(doer);
                        doerModel.AddChoresList(doer, userGroup);
                        return View(doerModel);
                    }
                    return InvalidGroup();
                }
                return DoerNotFound();
            }
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

        private ActionResult DoerNotFound()
        {
            TempData["FailureMessage"] = "The requested chore could not be found.";
            return RedirectToAction("Index");
        }
    }
}
