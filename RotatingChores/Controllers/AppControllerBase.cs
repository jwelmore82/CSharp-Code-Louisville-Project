﻿using RotatingChores.Extensions;
using RotatingChoresData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RotatingChores.Controllers
{
    public class AppControllerBase : Controller
    {
        protected Group GetUserGroup(RotatingChoresContext context)
        {
            var userGroupId = User.Identity.GetGroupId();
            Group userGroup = context.Groups.First(g => g.GroupId == userGroupId);
            return userGroup;
        }

        protected ActionResult NullIdEncountered()
        {
            TempData["FailureMessage"] = "You must specify an Id.";
            return RedirectToAction("Index", "Chores");
        }
    }
}