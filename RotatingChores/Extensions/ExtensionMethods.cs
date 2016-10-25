using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace RotatingChores.Extensions
{
    public static class ExtensionMethods
    {
        public static int GetGroupId(this IIdentity identity)
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(identity.GetUserId()).GroupId;
        }
    }
}