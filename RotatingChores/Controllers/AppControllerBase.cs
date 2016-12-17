using RotatingChores.Extensions;
using RotatingChoresData;
using System.Linq;
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

        protected ActionResult InvalidGroup()
        {
            TempData["FailureMessage"] = "You do not have permission to view or edit this item.";
            return RedirectToAction("Index", "Chores");
        }

        protected bool IsGroupObject(int objectGroupId)
        {
            if (objectGroupId == User.Identity.GetGroupId())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}