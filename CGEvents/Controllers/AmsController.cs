using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CGEvents.Models;

namespace CGEvents.Controllers
{
    public class AmsController : Controller
    {
        private readonly MiscFormsContext _context;

        public AmsController(MiscFormsContext context)
        {
            _context = context;
        }

        // GET: Ams
        public async Task<IActionResult> Index()
        {
            var miscFormsContext = _context.Ams.Include(a => a.Event);
            return View(await miscFormsContext.ToListAsync());
        }

        // GET: Ams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ams = await _context.Ams
                .Include(a => a.Event)
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
            ViewData["EventId"] = new SelectList(_context.EventMaster, "EventId", "EventName");
            return View();
        }

        // POST: Ams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fname,Lname,PassportName,Paname,Paemail,EmailId,EventId,EventGroupId,Company,UniqId,Patel,Atime,Adate,Dtime,Ddate,Id,AcityName,DcityName,IsRequired,OwnArrDate,OwnArrTime,OwnArrFlightNo,OwnDepDate,OwnDepTime,OwnDepFlightNo,HotelChkIn,HotelChkOut,IsVisaReq,DtSubmit,IsAttending,AttendingOpt,IsFollowupReq,IsHotelReq,Comments,IsTransferReq,Tempemail,ObflightNo,Obdate,Obetd,Obeta,Obclass,InflightNo,Indate,Inetd,Ineta,Inclass,Obsec,Insec,AirTktFileName,IsNew,IsVisaReqOpt,VisaFileName,City,Starter,Grill,Dessert,DtModified,NoOfCoAttendee,FdAllergy,AlleryDesc,AgendaFileName,IcsFileName,IndvDeadline,Position,ActualAttendance")] Ams ams)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ams);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.EventMaster, "EventId", "EventName", ams.EventId);
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
            ViewData["EventId"] = new SelectList(_context.EventMaster, "EventId", "EventName", ams.EventId);
            return View(ams);
        }

        // POST: Ams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Fname,Lname,PassportName,Paname,Paemail,EmailId,EventId,EventGroupId,Company,UniqId,Patel,Atime,Adate,Dtime,Ddate,Id,AcityName,DcityName,IsRequired,OwnArrDate,OwnArrTime,OwnArrFlightNo,OwnDepDate,OwnDepTime,OwnDepFlightNo,HotelChkIn,HotelChkOut,IsVisaReq,DtSubmit,IsAttending,AttendingOpt,IsFollowupReq,IsHotelReq,Comments,IsTransferReq,Tempemail,ObflightNo,Obdate,Obetd,Obeta,Obclass,InflightNo,Indate,Inetd,Ineta,Inclass,Obsec,Insec,AirTktFileName,IsNew,IsVisaReqOpt,VisaFileName,City,Starter,Grill,Dessert,DtModified,NoOfCoAttendee,FdAllergy,AlleryDesc,AgendaFileName,IcsFileName,IndvDeadline,Position,ActualAttendance")] Ams ams)
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
            ViewData["EventId"] = new SelectList(_context.EventMaster, "EventId", "EventName", ams.EventId);
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
                .Include(a => a.Event)
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
