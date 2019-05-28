using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGEvents.Models;
using Microsoft.EntityFrameworkCore;
namespace CGEvents.Views.Shared.Components
{
    public class Menu_UpcomingEvents : ViewComponent
    {
        private readonly MiscFormsContext _context;
        public Menu_UpcomingEvents(MiscFormsContext context)
        {
            _context = context;
        }

        public class Events
        {
            public int EventID { get; set; }
            public string EventName { get; set; }
            public DateTime? EventStartDate { get; set; }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await EventItemsAsync();
            return View(items);
        }

        public Task<List<Events>> EventItemsAsync()
        {
            return _context.EventMaster.Where(dt => dt.EventDate >= DateTime.Today).Select(f =>
               new Events { EventID = f.EventId, EventName = f.EventName, EventStartDate = f.EventDate }).ToListAsync();
        }
    }
}
