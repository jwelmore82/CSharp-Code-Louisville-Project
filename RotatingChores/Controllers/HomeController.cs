using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RotatingChoresData;
using RotatingChores.Models;
using System.Data.Entity;

namespace RotatingChores.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            //var rCContext = new RotatingChoresContext();

            //var group = new Group();

            //var litter = new Chore()
            //{
            //    Name = "Litter Box",
            //    Description = "Clean the kitty litter box.",
            //    Difficulty = 2,
            //    GroupId = group.GroupId

            //};

            //var choreDoer = new ChoreDoer()
            //{
            //    FirstName = "Josh",
            //    LastName = "Elmore",
            //    Email = "jwelmore82@gmail.com",
            //    GroupId = group.GroupId,
            //    MaxDifficulty = 5,
            //    Chores = new List<Chore>()
                
               
            //};

           
            //choreDoer.Chores.Add(litter);

            //rCContext.Groups.Add(group);
            //rCContext.Chores.Add(litter);
            //rCContext.ChoreDoers.Add(choreDoer);
            //rCContext.SaveChanges();

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}