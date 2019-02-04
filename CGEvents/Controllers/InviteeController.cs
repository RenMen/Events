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
using System.IO;


namespace CGEvents.Controllers
{
    public class InviteeController : Controller
    {
        private readonly MiscFormsContext _context;
       

        public InviteeController(MiscFormsContext context)
        {
            _context = context;
      
        }

        // GET: Ams/Index/
        //public IActionResult Index(int? id)
        //{

        //    return View();
        //}

        public IActionResult Index(short? eid)
        {
            if (eid == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = eid;
            ViewData["EventName"] = GetEventName(eid);
            return View();
        }

        //This class is used to limit sql column on both tables. see the sql profiler
        public class InviteeWithEventDetils
        {
            public short? EventId { get; set; }
            public int Id { get; set; }
            public string Fname { get; set; }
            public string Lname { get; set; }
            public string EventName { get; set; }
            public string Position { get; set; }
            public string Company { get; set; }
            public string EmailId { get; set; }
            public short? EventGroupID { get; set; }
            public DateTime? EventDateTo { get; set; }
            public DateTime? EventDate { get; set; }
            public short? EventGroupId { get; set; }
        }
        private IEnumerable<InviteeWithEventDetils> GetInvitee(int? id)
        {

            return _context.Ams
                 .Where(i => i.Id == id)
                 .Select(eve =>
                        new InviteeWithEventDetils { EventName = eve.Event.EventName, EventDate = eve.Event.EventDate, EventDateTo = eve.Event.EventDateTo, EventId = eve.EventId, Id = eve.Id, Fname = eve.Fname, Lname = eve.Lname, Position = eve.Position, Company = eve.Company, EmailId = eve.EmailId, EventGroupID = eve.EventGroupId }).ToList();   //.Select(event => EventMa ).ToList();

        }

        private IEnumerable<InviteeWithEventDetils> GetInvitees(int? eid)
        {
            return _context.Ams
            .Where(id => id.EventId == eid)
            .Select(eve =>
                    new InviteeWithEventDetils { EventName = eve.Event.EventName,EventDate=eve.Event.EventDate,EventDateTo=eve.Event.EventDateTo, EventId = eve.EventId, Id = eve.Id, Fname = eve.Fname, Lname = eve.Lname, Position = eve.Position, Company = eve.Company, EmailId = eve.EmailId, EventGroupID = eve.EventGroupId });   //.Select(event => EventMa ).ToList();

        }


        //private IEnumerable<Ams> GetInvitees1(int? eid)
        //{

        //    return _context.Ams.Include(e => e.Event)
        //       .Where(id => id.EventId == eid)
        //       .Select(eve =>
        //               new Ams { Event.EventName=eve.Event.EventName, EventId = eve.EventId, Id = eve.Id, Fname = eve.Fname, Lname = eve.Lname, Position = eve.Position, Company = eve.Company, EmailId = eve.EmailId, EventGroupId = eve.EventGroupId });   //.Select(event => EventMa ).ToList();

        //}

        public async Task<IActionResult> ReadInvitees([DataSourceRequest] DataSourceRequest request, int? eid, int? id)
        {

            if (eid == null && id == null)
            {
                return NotFound();
            }
            else if (eid != null && id == null)
            {
                ViewData["EventId"] = eid;
                //ViewData["EventName"] = GetEventName(eid);
                return Json(await GetInvitees(eid).ToDataSourceResultAsync(request));

            }
            else if (id != null)
            {
                return Json(await GetInvitee(id).ToDataSourceResultAsync(request));

            }
            else
            {
                return NotFound();
            }


        }


        // GET: Ams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ams = await _context.Ams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ams == null)
            {
                return NotFound();
            }

            return View(ams);
        }

        // GET: Ams/Create
        public IActionResult Create(short? eid)
        {
            if (eid == null)
            {
                return NotFound();
            }
            //ViewData["EventGroupId"] = GetNextGroupID(eid);
            ViewData["EventName"] = GetEventName(eid);
            ViewData["EventId"] = eid;
            return View();
        }
        public short? GetNextGroupID(short? eid)
        {
            return _context.Ams.Where(w => w.EventId == eid).Select(p => p.EventGroupId).Max(); 
        }

        public string GetEventName(short? eid)
        {
            //  return _context.Ams.Include(s=>s.Event).Where(w => w.EventId == eid).FirstOrDefault().Event.EventName;
            return _context.EventMaster.Where(w => w.EventId == eid).FirstOrDefault().EventName;
        }

        // POST: Ams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,EventGroupId,Fname,Lname,EmailId,EventId,Company,Position,IndvDeadline,")] Ams ams)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(ams);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(ams);
        //}
        #region Grid Batch binding
        //**********https://docs.telerik.com/aspnet-core/html-helpers/data-management/grid/editing/batch-editing.html *********
        //****************

        public async Task<ActionResult> Invitee_Read([DataSourceRequest]DataSourceRequest request,short? eid)
        {
            //ToDataSourceResult works with IEnumerable and IQueryable
            
            IEnumerable<Ams> Invitees = await _context.Ams.Where(i => i.EventId == eid && i.EventGroupId==-1).ToListAsync(); // EventGroupId=-1 inorder to return empty row
            DataSourceResult result = Invitees.ToDataSourceResult(request);
               return Json(result);
            
        }

        public async Task<ActionResult> Invitee_Create([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Ams> ams)
        {
            var entities = new List<Ams>();
            try
            {
                // Will keep the inserted entitites here. Used to return the result later.
                
                if (ModelState.IsValid)
                {
                    foreach (var Invitee in ams)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel.
                        var entity = new Ams
                        {
                            Fname = Invitee.Fname,
                            Lname = Invitee.Lname,
                            EmailId = Invitee.EmailId,
                            Position = Invitee.Position,
                            IndvDeadline = Invitee.IndvDeadline,
                            Company = Invitee.Position,
                            EventGroupId = GetNextGroupID(Invitee.EventId) == null ? 1 : (short?)(GetNextGroupID(Invitee.EventId) + 1)

                        };
                        // Add the entity.
                        _context.Ams.Add(entity);
                        // Store the entity for later use.
                        entities.Add(entity);
                    }
                    // Insert the entities in the database.
                    _context.SaveChanges();
                    //}
                }

                // Return the inserted entities. The Grid needs the generated ProductID. Also return any validation errors.
                return Json(await entities.ToDataSourceResultAsync(request, ModelState, Invitee => new Ams
                {
                    Fname = Invitee.Fname,
                    Lname = Invitee.Lname,
                    EmailId = Invitee.EmailId,
                    Position = Invitee.Position,
                    IndvDeadline = Invitee.IndvDeadline,
                    Company = Invitee.Position,
                    EventGroupId = Invitee.EventGroupId
                }));
            }
            catch (Exception e)
            {
                //*********************************
                //
                // https://stackoverflow.com/questions/17790107/how-to-return-modelstate-errors-to-kendo-grid-in-mvc-web-api-post-method
                //
                //****************************
                var messageSplit = e.InnerException.Message.Split("(");
                var messageToClient = "";

                if (e.InnerException.Message.ToLower().Contains("unique key constraint"))
                {
                    messageToClient = "Please remove duplicate email " + messageSplit[1].Split(",")[0] + " and click 'Save Changes' ";
                }
                else
                {
                    messageToClient = e.InnerException.Message;
                }
               
                ModelState.AddModelError(string.Empty, messageToClient);
                // Return the inserted entities. The Grid needs the generated ProductID. Also return any validation errors.
                return Json(await entities.ToDataSourceResultAsync(request, ModelState, Invitee => new Ams
                {
                    Fname = Invitee.Fname,
                    Lname = Invitee.Lname,
                    EmailId = Invitee.EmailId,
                    Position = Invitee.Position,
                    IndvDeadline = Invitee.IndvDeadline,
                    Company = Invitee.Position,
                    EventGroupId = Invitee.EventGroupId
                }));
            }
            
        }

        public async Task<ActionResult> Invitee_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Ams> ams)
        {

            // Will keep the inserted entitites here. Used to return the result later.
            var entities = new List<Ams>();
            if (ModelState.IsValid)
            {
                foreach (var Invitee in ams)
                {
                    // Create a new Product entity and set its properties from the posted ProductViewModel.
                    var entity = new Ams
                    {
                        Fname = Invitee.Fname,
                        Lname = Invitee.Lname,
                        EmailId = Invitee.EmailId,
                        Position = Invitee.Position,
                        IndvDeadline = Invitee.IndvDeadline,
                        Company = Invitee.Position,
                        EventGroupId = Invitee.EventGroupId

                    };
                    // Store the entity for later use.
                    entities.Add(entity);
                    // Attach the entity.
                    _context.Ams.Attach(entity);
                    // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one.
                    _context.Entry(entity).State = EntityState.Modified;
                    // Or use ObjectStateManager if using a previous version of Entity Framework.
                    // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
                }
                // Update the entities in the database.
                _context.SaveChanges();
            }

            // Return the updated entities. Also return any validation errors.

            return Json(await entities.ToDataSourceResultAsync(request, ModelState, Invitee => new Ams
            {
                Fname = Invitee.Fname,
                Lname = Invitee.Lname,
                EmailId = Invitee.EmailId,
                Position = Invitee.Position,
                IndvDeadline = Invitee.IndvDeadline,
                Company = Invitee.Position,
                EventGroupId = Invitee.EventGroupId
            }));

        }
        public async Task<ActionResult> Invitee_Destroy([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Ams> ams)
        {
             // Will keep the inserted entitites here. Used to return the result later.
            var entities = new List<Ams>();
            if (ModelState.IsValid)
            {
                foreach (var Invitee in ams)
                {
                    // Create a new Product entity and set its properties from the posted ProductViewModel.
                    var entity = new Ams
                    {
                        Fname = Invitee.Fname,
                        Lname = Invitee.Lname,
                        EmailId = Invitee.EmailId,
                        Position = Invitee.Position,
                        IndvDeadline = Invitee.IndvDeadline,
                        Company = Invitee.Position

                    };
                    // Store the entity for later use.
                    entities.Add(entity);
                    // Attach the entity.
                    _context.Ams.Attach(entity);
                    // Delete the entity.
                    _context.Ams.Remove(entity);
                        // Or use DeleteObject if using a previous versoin of Entity Framework.
                        // northwind.Products.DeleteObject(entity);
                    }
                // Delete the entity in the database.
                _context.SaveChanges();
                }
       
            // Return the destroyed entities. Also return any validation errors.
            return Json(await entities.ToDataSourceResultAsync(request, ModelState, Invitee => new Ams
            {
                Fname = Invitee.Fname,
                Lname = Invitee.Lname,
                EmailId = Invitee.EmailId,
                Position = Invitee.Position,
                IndvDeadline = Invitee.IndvDeadline,
                Company = Invitee.Position,
                EventGroupId = Invitee.EventGroupId
            }));
        }
        #endregion
        // GET: Ams/Edit/5
        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ams = await _context.Ams.Include(e => e.Event).FirstOrDefaultAsync(i => i.Id == id);
            // var ams = await _context.Ams.FindAsync(id);
            if (ams == null)
            {
                return NotFound();
            }
            return View(ams);
        }

        // POST: Ams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fname,Lname,EmailId,EventId,Company,IndvDeadline,Position")] Ams ams)
        {

            if (id != ams.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ams);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmsExists(ams.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Invitee", new { eid = ams.EventId });
            }
            // return RedirectToAction(nameof(Index));
            ams = await _context.Ams.Include(e => e.Event).FirstOrDefaultAsync(i => i.Id == id);
            return View(ams);
        }

        // GET: Ams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ams = await _context.Ams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ams == null)
            {
                return NotFound();
            }

            return View(ams);
        }

        // POST: Ams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ams = await _context.Ams.FindAsync(id);
            _context.Ams.Remove(ams);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmsExists(int id)
        {
            return _context.Ams.Any(e => e.Id == id);
        }

       


    }
}

    

