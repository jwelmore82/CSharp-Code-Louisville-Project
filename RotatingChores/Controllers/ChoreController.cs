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
using System.Net;

namespace RotatingChores.Controllers
{
    [Authorize]
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var choreContext = new RotatingChoresContext())
            {
                var choreModel = ChoreModel.ConvertFromChore(choreContext.Chores.SingleOrDefault(c => c.ChoreId == id));
                if (choreModel != null)
                {
                    return View(choreModel);
                }                
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var choreContext = new RotatingChoresContext())
            {
                var choreModel = ChoreModel.ConvertFromChore(choreContext.Chores.SingleOrDefault(c => c.ChoreId == id));
                if (choreModel != null)
                {
                    SetGroupMemberSelectList();
                    return View(choreModel);
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(ChoreModel model)
        {
            using (var context = new RotatingChoresContext())
            {
                var chore = model.GetRepresentedChore(context);
                
                model.UpdateChore(context, chore);                
                ValidateAssignTo(chore);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            SetGroupMemberSelectList();
            return View(model);
        }

        public ActionResult Add()
        {
            SetGroupMemberSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(ChoreModel newChore)
        {
            
            using (var context = new RotatingChoresContext())
            {
                var addingChore = context.Chores.Create();
                newChore.UpdateChore(context, addingChore);
                                
                ValidateAssignTo(addingChore);
                if (ModelState.IsValid)
                {
                    addingChore.GroupId = User.Identity.GetGroupId();
                    context.Chores.Add(addingChore);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                SetGroupMemberSelectList();
                return View(newChore);
            }

            
        }

        private void SetGroupMemberSelectList()
        {
            var returnList = new List<ChoreDoerModel>();
            using (var context = new RotatingChoresContext())
            {
                var userGroupId = User.Identity.GetGroupId();
                Group userGroup = context.Groups.First(g => g.GroupId == userGroupId);
                if (userGroup.Members.Count() > 0)
                {
                    foreach (var member in userGroup.Members)
                    {
                        returnList.Add(ChoreDoerModel.ConvertFromDoer(member));
                    }
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
    }
}