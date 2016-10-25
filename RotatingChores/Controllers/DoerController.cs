using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RotatingChores.Models;
using RotatingChoresData;

namespace RotatingChores.Controllers
{
    public class DoerController : Controller
    {
        // GET: Doer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ChoreDoerModel model)
        {
            
            return View();
        }
    }
}
