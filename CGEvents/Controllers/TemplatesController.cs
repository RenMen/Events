using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CGEvents.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;

namespace CGEvents.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly MiscFormsContext _context;

        public TemplatesController(MiscFormsContext context)
        {
            _context = context;
        }

        class IntimationTemplate
        {
            public short? EventId { get; set; }            
            public string TemplateName { get; set; }
            public String Subject { get; set; }
            public String Type { get; set; }
            public int? ID { get; set; }
            public virtual EventMaster Event { get; set; }
            public virtual IntimationTypeMaster IntimationType { get; set; }
        }

        public class IntimationTemplateType
        {
            public string IntimationType { get; set; }
            public byte? IntimationTypeId { get; set; }
        }

       public class IntimationTemplateHTML
        {
            public string IntimationRawHTML { get; set; }
            public byte? IntimationTypeId { get; set; }
            public string IntimationRawHTMLName { get; set; }
            public int? ID{ get; set; }
        }

        // GET: Templates
        public IActionResult Index(int? id)
        {
            //var miscFormsContext = _context.IntimationTemplateMaster.Include(i => i.Event).Include(i => i.IntimationType).Where(id=>id.EventId==evtid);
            //return View(await miscFormsContext.ToListAsync());
           
            if (id== null)
            {
                return NotFound();
            }
            else
            {
                ViewData["EventId"] = id;
                ViewData["EventName"] = getEventName(id);
                return View();
            }
        }


        public async Task<IActionResult> ReadTemplates([DataSourceRequest] DataSourceRequest request, int? evtid)
        {
           

            if (evtid == null )
            {
                return NotFound();
            }
            else if (evtid != null )
            {
               
                return Json(await GetTemplates(evtid).ToDataSourceResultAsync(request)); ;

            }
              else
            {
                return NotFound();
            }


        }
        private IEnumerable<IntimationTemplate> GetTemplates(int? evtid)
        {
       //use linqpad to create the below complex lambda expression
                    return _context.IntimationTemplateMaster
                         .Where(u => u.EventId == evtid)                         
                             .Select(
                                eve =>
                                   new IntimationTemplate
                                   {                                       
                                       Event = eve.Event,
                                       IntimationType=eve.IntimationType,
                                       EventId = eve.EventId,
                                       TemplateName=eve.TemplateName,
                                       Subject=eve.Subject,
                                       Type=eve.IntimationType.IntimationType,
                                       ID=eve.Id 
                                      
                                     // InvTypeID=intimation.IntimationTypeId
                                 }
                             );

                
            }
        

        // GET: Templates/Details/5
        public async Task<IActionResult> Details(int? evtid)
        {
            if (evtid == null)
            {
                return NotFound();
            }

            var intimationTemplateMaster = await _context.IntimationTemplateMaster
                .Include(i => i.Event)
                .Include(i => i.IntimationType)
                .FirstOrDefaultAsync(m => m.EventId == evtid);
            if (intimationTemplateMaster == null)
            {
                return NotFound();
            }

            return View(intimationTemplateMaster);
        }

        public string getEventName(int? eid)
        {
            return _context.EventMaster.Where(e => e.EventId == eid).Select(e => e.EventName).FirstOrDefault();
             
        }
       // [HttpGet]
       // [Route("Templates/Create/{EventID}")]
        // GET: Templates/Create
        public IActionResult Create(int? EventID)
        {
            //ViewData["EventId"] = new SelectList(_context.EventMaster, "EventId", "EventName");
            //ViewData["IntimationTypeId"] = new SelectList(_context.IntimationTypeMaster, "IntimationTypeId", "IntimationType");
            ViewData["EventName"] = getEventName(EventID);
            return View();
        }
        public ActionResult GetTemplateTypes()
        {
            var TemplatesType = _context.IntimationTypeMaster.Select(e => new IntimationTemplateType
            {
                IntimationType = e.IntimationType,
                IntimationTypeId = e.IntimationTypeId
            });

            return Json(TemplatesType);
        }

        public ActionResult GetRawHTMLTemplateNames(int id)
        {
            var TemplatesHTML = _context.IntimationRawHTMLMaster.Select(e => new IntimationTemplateHTML
            {
               ID=e.ID,
               IntimationTypeId = e.IntimationTypeID,
               IntimationRawHTMLName=e.IntimationRawHTMLName

            }).Where(i=>i.IntimationTypeId==id);

            return Json(TemplatesHTML);
        }


        public ActionResult GetRawHTML(int id)
        {
            var RawHTML = _context.IntimationRawHTMLMaster.Where(i=>i.ID==id).Select(e => new IntimationTemplateHTML
            {
                ID = e.ID,
                IntimationRawHTML = e.IntimationRawHTML
            });

            return Json(RawHTML);
        }
        // POST: Templates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Templates/Create/{EventID}")]
        public async Task<IActionResult> Create([Bind("Id,IntimationTypeId,HtmlContent,MergeFields,EventId,TemplateName,Subject")] IntimationTemplateMaster intimationTemplateMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(intimationTemplateMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new{id= intimationTemplateMaster.EventId });
            }
           // ViewData["EventId"] = new SelectList(_context.EventMaster, "EventId", "EventName", intimationTemplateMaster.EventId);
           // ViewData["IntimationTypeId"] = new SelectList(_context.IntimationTypeMaster, "IntimationTypeId", "IntimationTypeId", intimationTemplateMaster.IntimationTypeId);
            return View(intimationTemplateMaster);
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intimationTemplateMaster = await _context.IntimationTemplateMaster.FindAsync(id);
            if (intimationTemplateMaster == null)
            {
                return NotFound();
            }
            // ViewData["EventId"] = new SelectList(_context.EventMaster, "EventId", "EventName", intimationTemplateMaster.EventId);
            // ViewData["IntimationTypeId"] = new SelectList(_context.IntimationTypeMaster, "IntimationTypeId", "IntimationTypeId", intimationTemplateMaster.IntimationTypeId);
            ViewData["EventId"] = intimationTemplateMaster.EventId;
            ViewData["EventName"] = getEventName(intimationTemplateMaster.EventId);
            return View(intimationTemplateMaster);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IntimationTypeId,EventId,Subject,HtmlContent,TemplateName")]  Models.IntimationTemplateMaster intimationTemplateMaster)
        {
            if (id != intimationTemplateMaster.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(intimationTemplateMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntimationTemplateMasterExists(intimationTemplateMaster.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = intimationTemplateMaster.EventId });
            }
            ViewData["EventId"] = intimationTemplateMaster.EventId;
            //ViewData["IntimationTypeId"] = new SelectList(_context.IntimationTypeMaster, "IntimationTypeId", "IntimationTypeId", intimationTemplateMaster.IntimationTypeId);
            return View(intimationTemplateMaster);
        }

        // GET: Templates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intimationTemplateMaster = await _context.IntimationTemplateMaster
                .Include(i => i.Event)
                .Include(i => i.IntimationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (intimationTemplateMaster == null)
            {
                return NotFound();
            }

            return View(intimationTemplateMaster);
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intimationTemplateMaster = await _context.IntimationTemplateMaster.FindAsync(id);
            _context.IntimationTemplateMaster.Remove(intimationTemplateMaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IntimationTemplateMasterExists(int id)
        {
            return _context.IntimationTemplateMaster.Any(e => e.Id == id);
        }
    }
}
