﻿using Microsoft.AspNet.Identity;
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
            SetGroupMemberEnum();
            return View();
        }

        [HttpPost]
        public ActionResult Add(ChoreModel newChore)
        {
            
            using (var context = new RotatingChoresContext())
            {
                var addingChore = newChore.ConvertToChore(context);

                addingChore.GroupId = User.Identity.GetGroupId();
                context.Chores.Add(addingChore);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private void SetGroupMemberEnum()
        {
            var returnList = new List<ChoreDoer>();
            using (var context = new RotatingChoresContext())
            {
                var userGroupId = User.Identity.GetGroupId();
                Group userGroup = context.Groups.First(g => g.GroupId == userGroupId);
                if (userGroup.Members.Count() > 0)
                {
                    foreach (var member in userGroup.Members)
                    {
                        returnList.Add(member);
                    }
                }
            }
            var selectList = new SelectList(returnList, "ChoreDoerId", "Name", "Assign To");
            ViewBag.GroupMembers = selectList;
        }
    }
}