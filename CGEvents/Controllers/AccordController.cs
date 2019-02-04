using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CGEvents.Controllers
{
    public class AccordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}