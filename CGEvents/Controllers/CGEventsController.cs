using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CGEvents.Controllers
{
    [Route("CGEvents")]
    public class CGEventsController : Controller
    {
        //[Route("{view=Index}")]
        public IActionResult Index(string view)
        {
            ViewData["Title"] = "Event Management";

            return View(view);
        }
    }
}