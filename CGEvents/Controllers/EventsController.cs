using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CGEvents.Models;
using System.Security.Principal;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json.Serialization;
namespace CGEvents.Controllers
{

    public class EventsController : Controller
    {
        private readonly MiscFormsContext _context;

       
        public EventsController(MiscFormsContext context)
        {
            _context = context;

        }


        private IEnumerable<EventMaster> GetEvents()
        {

           return _context.EventMaster                
                .Select(eve =>
                       new EventMaster {EventId=eve.EventId, EventName = eve.EventName,Venue=eve.Venue,AckText=eve.AckText }).ToList();   //.Select(event => EventMa ).ToList();
           
         }

        private IEnumerable<EventMaster> GetEvents(int? eid)
        {
            return _context.EventMaster
                .Where(id=>id.EventId==eid)
                .Select(eve =>
                   new EventMaster { EventId = eve.EventId, EventName = eve.EventName, Venue = eve.Venue, AckText = eve.AckText }).ToList();   //.Select(event => EventMa ).ToList();
        }



        public async Task<IActionResult> ReadEvents([DataSourceRequest] DataSourceRequest request, int? eid)
        {
            if (eid != null)
            {
                return Json(await GetEvents(eid).ToDataSourceResultAsync(request));

            }
            else
            {
                return Json(await GetEvents().ToDataSourceResultAsync(request));

            }
        }
        // GET: EventMasters
        public IActionResult Index()
        {
            return View();                    

        }
        

        // GET: EventMasters/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventMaster = await _context.EventMaster
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventMaster == null)
            {
                return NotFound();
            }

            return View(eventMaster);
        }

        // GET: EventMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDispName,EventDate,FormDeadline,AckText,Venue,Hotel,EventDateTo,Subject,MailHeader,MailBody,MailSignature,IsActive")] EventMaster eventMaster)
        {
            if (ModelState.IsValid)
            {
                eventMaster.IsActive = true; //eventMaster.NullableBooleanPropertyProxy;
                _context.Add(eventMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventMaster);
        }

        // GET: EventMasters/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventMaster = await _context.EventMaster.FindAsync(id);
            if (eventMaster == null)
            {
                return NotFound();
            }
            return View(eventMaster);
        }

        // POST: EventMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("EventId,EventName,EventDispName,EventDate,FormDeadline,AckText,Venue,Hotel,EventDateTo,Subject,MailHeader,MailBody,MailSignature,IsActive")] EventMaster eventMaster)
        {
           
            if (id != eventMaster.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  //  eventMaster.IsActive = NullableBooleanPropertyProxy;
                    _context.Update(eventMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventMasterExists(eventMaster.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventMaster);
        }

        // GET: EventMasters/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventMaster = await _context.EventMaster
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventMaster == null)
            {
                return NotFound();
            }

            return View(eventMaster);
        }

        // POST: EventMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var eventMaster = await _context.EventMaster.FindAsync(id);
            _context.EventMaster.Remove(eventMaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventMasterExists(short id)
        {
            return _context.EventMaster.Any(e => e.EventId == id);
        }
    }
}
