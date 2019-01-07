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
        public IActionResult Index( int? id)
        {
           
           return View();
        }



        private IEnumerable<Ams> GetInvitee(int? id)
        {

            return _context.Ams
                 .Where(i => i.Id == id)
                 .Select(eve =>
                        new Ams {Id=eve.Id, Fname = eve.Fname, Lname = eve.Lname, Position = eve.Position, Company = eve.Company ,EmailId=eve.EmailId,EventGroupId=eve.EventGroupId}).ToList();   //.Select(event => EventMa ).ToList();

        }

        private IEnumerable<Ams> GetInvitees(int? eid)
        {
            return _context.Ams
                .Where(id => id.EventId == eid)
                .Select(eve =>
                        new Ams {Id=eve.Id, Fname = eve.Fname, Lname = eve.Lname, Position = eve.Position, Company = eve.Company, EmailId = eve.EmailId, EventGroupId = eve.EventGroupId }).ToList();   //.Select(event => EventMa ).ToList();
        }

               

        public async Task<IActionResult> ReadInvitees([DataSourceRequest] DataSourceRequest request, int? eid,int? id)
        {

            if (eid == null && id == null)
            {
                return NotFound();
            }
            else if (eid != null && id == null)
            {
                return Json(await GetInvitees(eid).ToDataSourceResultAsync(request));

            }
            else if (id != null)
            {
                return Json(await GetInvitee(id).ToDataSourceResultAsync(request));

            }
            else {
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fname,Lname,PassportName,Paname,Paemail,EmailId,EventId,Company,Patel,IndvDeadline,Position")] Ams ams)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ams);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ams);
        }

        // GET: Ams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ams = await _context.Ams.FindAsync(id);
            if (ams == null)
            {
                return NotFound();
            }
            return View(ams);
        }

        // POST: Ams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fname,Lname,PassportName,Paname,Paemail,EmailId,EventId,Company,Patel,IndvDeadline,Position")] Ams ams)
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
                return RedirectToAction(nameof(Index));
            }
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
