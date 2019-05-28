using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CGEvents.Models;
using System.Diagnostics;
namespace CGEvents.Controllers
{
    public class EmailController : Controller
    {
        private readonly MiscFormsContext _context;
        public EmailController(MiscFormsContext context)
        {
            _context = context;
            
        }

        public class EventDropDownModel
        {
            public string EventName { get; set; }
            public int EventId { get; set; }
        }

        public class TemplateDropDownModel
        {
            public int ID { get; set; }
            public string Filename { get; set; }
        }

        public class FilterDropDownModel
        {
            public short IntimationGroupID { get; set; }
            public string IntimationGroupName { get; set; }
        }

        public IActionResult Index(short? eid)
        {
            ViewData["EventId"] = eid;
            ViewData["EventName"] = GetEventName(eid);
            return View("SaveTheDate");
        }
        public string GetEventName(short? eid)
        {
            if (eid == null)
               {
                //  return _context.Ams.Include(s=>s.Event).Where(w => w.EventId == eid).FirstOrDefault().Event.EventName;
                return _context.EventMaster.FirstOrDefault().EventName;
            }
            else {
                //  return _context.Ams.Include(s=>s.Event).Where(w => w.EventId == eid).FirstOrDefault().Event.EventName;
                return _context.EventMaster.Where(w => w.EventId == eid).FirstOrDefault().EventName;
            }

        }
        public ActionResult GetEvents()
        {
            var EventsDropDownList = _context.EventMaster.Where(id => id.EventDate >= DateTime.Today).Select(e => new EventDropDownModel
            {
                EventName = e.EventName,
                EventId = e.EventId
            });

            return Json(EventsDropDownList);
        }

        
        public ActionResult GetTemplates()
        {


            //Save the date typeid==1

            var TemplatesDropDownList = _context.IntimationTemplateMaster.Where(typeid => typeid.IntimationTypeId == 1).Select(e => new TemplateDropDownModel
            {
                Filename = e.TemplateName,
                ID = e.Id
            });

            //Debug.WriteLine(Json(TemplatesDropDownList));
            return Json(TemplatesDropDownList);
        }
        public ActionResult GetFilter()
        {
            //Save the date typeid==1
            var FilterDropDownList = _context.IntimationGroupMaster.Where(gid => gid.IntimationGroupTypeAssociation.Any(i=>i.IntimationTypeId==1)).Select(e => new FilterDropDownModel
            {
                IntimationGroupID = e.IntimationGroupId,
                IntimationGroupName = e.IntimationGroupName
            });

            return Json(FilterDropDownList);
        }

    }
}