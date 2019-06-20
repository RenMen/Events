using CGEvents.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace CGEvents.Views.Shared.Components
{
    public class SendMailType:ViewComponent
    {
        private readonly MiscFormsContext _context;
        public SendMailType(MiscFormsContext context)
        {
            _context = context;
        }

        public class IntimationTypes
        {
            public int IntimationTypeID { get; set; }
            public string IntimationType { get; set; }
            public string IntimationIconClass { get; set; }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await IntimationTypesAsync();
            return View(items);
        }

        public Task<List<IntimationTypes>> IntimationTypesAsync()
        {
            return _context.IntimationTypeMaster.Where(t =>t.IntimationType != null).Select(f =>
               new IntimationTypes { IntimationTypeID = f.IntimationTypeId, IntimationType = f.IntimationType,IntimationIconClass=f.IntimationIconClass}).ToListAsync();
        }
    }
}
