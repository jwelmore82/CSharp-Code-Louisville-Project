﻿using RotatingChores.Models;
using RotatingChoresData;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RotatingChores.Extensions;

namespace RotatingChores.Controllers
{
    [Authorize]
    public class ChoresController : AppControllerBase
    {
        // GET: Chore
        public ActionResult Index()
        {
            using (var context = new RotatingChoresContext())
            {
                var group = GetUserGroup(context);
                //Check to make sure the group is found and has chores
                if (group != null & group.Chores.Count > 0)
                {
                    //"Index" model is of type List<ChoreModel>
                    List<ChoreModel> chores = new List<ChoreModel>();
                    foreach (var chore in group.Chores)
                    {
                        chores.Add(ChoreModel.ConvertFromChore(chore));
                    }
                    return View(chores);
                }
            }
            //If there are no chores in the group redirect to Add page;
            TempData["Message"] = "No chores yet, add one now!";
            return RedirectToAction("Add");
        }

        public ActionResult Detail(int? id)
        {
            return ViewChoreById(id);
        }

        public ActionResult Edit(int? id)
        {
            return ViewChoreById(id);
        }

        

        [HttpPost]
        public ActionResult Edit(ChoreModel model)
        {
            using (var context = new RotatingChoresContext())
            {
                var chore = model.GetRepresentedChore(context);
                
                model.UpdateChore(context, chore);                
                ValidateAssignTo(chore);
                if (ModelState.IsValid && IsGroupObject(chore.GroupId))
                {
                    context.SaveChanges();
                    TempData["Message"] = "Chore has been updated!";
                    return RedirectToAction("Index");
                }
                SetGroupMemberSelectList(context);
                return View(model);
            }
            
        }

        public ActionResult Add()
        {
            using (var context = new RotatingChoresContext())
            {
                SetGroupMemberSelectList(context);
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult Add(ChoreModel choreModel)
        {
            
            using (var context = new RotatingChoresContext())
            {
                var addingChore = context.Chores.Create();
                choreModel.UpdateChore(context, addingChore);
                                
                ValidateAssignTo(addingChore);
                if (ModelState.IsValid)
                {
                    addingChore.GroupId = User.Identity.GetGroupId();
                    context.Chores.Add(addingChore);
                    context.SaveChanges();
                    TempData["Message"] = "New chore added!";
                    return RedirectToAction("Index");
                }

                //If there is a problem with the model
                SetGroupMemberSelectList(context);
                return View(choreModel);
            }

            
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {

            return ViewChoreById(id);

            
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            using (var context = new RotatingChoresContext())
            {

                var chore = GetChoreById(id, context);
                if (chore != null && IsGroupObject(chore.GroupId))
                {
                    context.Chores.Remove(chore);
                    context.SaveChanges();
                    TempData["Message"] = "Chore successfully deleted!";
                    return RedirectToAction("Index");
                }
                
                TempData["FailureMessage"] = "There was an issuse deleting the chore.";
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult MarkComplete(int? id)
        {
            return ViewChoreById(id);

        }

        [HttpPost, ActionName("MarkComplete")]
        public ActionResult MarkCompletePost(int? id)
        {
            using (var context = new RotatingChoresContext())
            {

                var chore = GetChoreById(id, context);
                if (chore != null && IsGroupObject(chore.GroupId))
                {
                    var group = GetUserGroup(context);
                    //Working with the model
                    var choreModel = ChoreModel.ConvertFromChore(chore);
                    //Change LastCompleted etc on model
                    choreModel.MarkComplete();
                    //Use model to update chore
                    choreModel.UpdateChore(context, chore);
                    //Assign chore to next available person.
                    choreModel.AdvanceChore(chore, group);
                    context.SaveChanges();
                    TempData["Message"] = "Chore marked complete! Good Job!";
                    return RedirectToAction("Index");
                }

                TempData["FailureMessage"] = "There was an issuse completing the chore.";
                return RedirectToAction("Index");
            }
        }


        //The following are the methods used in this controller.
        //*****************************************************************************************

        //Returns a view with a ChoreModel if all checks pass.
        private ActionResult ViewChoreById(int? id)
        {
            //Check for existing Id
            if (id == null)
            {
                return NullIdEncountered();
            }

            using (var context = new RotatingChoresContext())
            {
                var chore = GetChoreById(id, context);
                //Check for chore in database
                if (chore != null)
                {
                    //Check if Chore is in user's group
                    if (IsGroupObject(chore.GroupId))
                    {
                        var choreModel = ChoreModel.ConvertFromChore(chore);
                        SetGroupMemberSelectList(context);
                        return View(choreModel);
                    }
                    return InvalidGroup();
                }
                return ChoreNotFound();
            }
        }

        private void SetGroupMemberSelectList(RotatingChoresContext context)
        {
            var returnList = new List<ChoreDoerModel>();                       
            Group userGroup = GetUserGroup(context);
            if (userGroup.Members.Count() > 0)
            {
                foreach (var member in userGroup.Members)
                {
                    returnList.Add(ChoreDoerModel.ConvertFromDoer(member));
                }
            }            
            var selectList = new SelectList(returnList, "ChoreDoerId", "Name");
            ViewBag.GroupMembers = selectList;
        }


        //Prevents a Chore from being assigned to a ChoreDoer with a lower MaxDifficulty
        private void ValidateAssignTo(Chore chore)
        {
            if (chore.Difficulty > chore.AssignedTo.MaxDifficulty)
            {
                ModelState.AddModelError("Difficulty", "This chore is too difficult for the selected user. Change Difficulty or raise MaxDifficulty for the user.");
            }
        }

        //Gets the Chore using ChoreId 
        private static Chore GetChoreById(int? id, RotatingChoresContext context)
        {
            if (id == null)
            {
                return null;
            }
            return context.Chores.SingleOrDefault(c => c.ChoreId == id);
        }



        //Use when ChoreId valid but not found( e.g. SingleOrDefault returns null).
        private ActionResult ChoreNotFound()
        {
            TempData["FailureMessage"] = "The requested chore could not be found.";
            return RedirectToAction("Index");
        }
    }
}