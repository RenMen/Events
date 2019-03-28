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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace CGEvents.Controllers
{

    public class EventsController : Controller
    {
        private readonly MiscFormsContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly  IHostingEnvironment _hostingEnvironment;

        public IDirectoryContents DirectoryContents { get; private set; }


        class UploadInitialFile
        {
            public string filename { get; set; }
            public long size { get; set; }
           

        }
        



        public EventsController(MiscFormsContext context, IFileProvider fileProvider, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _fileProvider = fileProvider;
            _hostingEnvironment = hostingEnvironment;

        }


        private IEnumerable<Models.EventMaster> GetEvents()
        {
            try
            {
                          

           return _context.EventMaster                
                .Select(eve =>
                       new EventMaster {EventId=eve.EventId, EventName = eve.EventName,EventDate=eve.EventDate,EventDateTo=eve.EventDateTo }).ToList();   //.Select(event => EventMa ).ToList();
            
            }
            catch ( Exception e) 
            {

               Console.Write( e.Message);
                return null;
            }
        }

        private IEnumerable<Models.EventMaster> GetEvents(int? eid)
        {
            return  _context.EventMaster
                .Where(id=>id.EventId==eid)
                .Select(eve =>
                   new EventMaster { EventId = eve.EventId, EventName = eve.EventName, EventDate = eve.EventDate, EventDateTo = eve.EventDateTo }).ToList();   //.Select(event => EventMa ).ToList();

           
        }



        public async Task<IActionResult> ReadEvents([DataSourceRequest] DataSourceRequest request, int? eid)
        {
            try
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
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
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
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDispName,EventDate,FormDeadline,AckText,Venue,Hotel,EventDateTo,Subject,MailHeader,MailBody,MailSignature,IsActive")] Models.EventMaster eventMaster)
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
        public async Task<IActionResult> Edit(short id, [Bind("EventId,EventName,EventDispName,EventDate,FormDeadline,AckText,Venue,Hotel,EventDateTo,Subject,MailHeader,MailBody,MailSignature,IsActive")] Models.EventMaster eventMaster)
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
