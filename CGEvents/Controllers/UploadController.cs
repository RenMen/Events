using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CGEvents.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CGEvents.Controllers
{
   
    public class UploadController : Controller
    {
        private IHostingEnvironment _env;
        private readonly MiscFormsContext _context;

        public class MandatoryColumns
        {
            public int colIndex;
            public string columnName;
        }
        public class ColumnsToDB
        {
            public string Fname;
            public string Lname;
            public string Email;
            public string Position;
            public string Company;
            public string EventGroupId;
            public string EventId;
            public DateTime IndividualDeadline;
        }
        public UploadController(MiscFormsContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }


        // GET: Upload
        public async Task<IActionResult> Index()
        {
            //var miscFormsContext = _context.Ams.Include(a => a.EventIdNavigation);
            //return View(await miscFormsContext.ToListAsync());
            return View();
        }

        // GET: Upload/Details/5
        public ActionResult Async_Save(IEnumerable<IFormFile> files)
        {

            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _env.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                bool splitcol=false; //if file has no lname column then trigger the module to split the fname based on space character and create last name column
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<table class='table'><tr>");
                    sb.Append("<th>Row#</th><th>Error Message</th>");

                    //****mandatory columns
                    //var emailCol = 0;
                    //var fname = 0;
                    //var lname = 0;
                    //var position = 0;
                    //var company = 0;
                    IList<MandatoryColumns> MColumnList = new List<MandatoryColumns>();

                    for (int j = 0; j < cellCount; j++)
                    {

                        ICell cell = headerRow.GetCell(j);


                        /********  Used to check whether mandatory column names exist*/
                        if (cell.ToString().ToLower().Contains("mail"))
                        {
                            MColumnList.Add(new MandatoryColumns() { colIndex = j, columnName = "email" });
                        }
                        else if (cell.ToString().ToLower().Contains("fname") || cell.ToString().ToLower().Contains("first"))
                        {
                            MColumnList.Add(new MandatoryColumns() { colIndex = j, columnName = "fname" });
                        }
                        else if (cell.ToString().ToLower().Contains("lname") || cell.ToString().ToLower().Contains("last"))
                        {
                            MColumnList.Add(new MandatoryColumns() { colIndex = j, columnName = "lname" });
                        }
                        else if (cell.ToString().ToLower().Contains("position"))
                        {
                            MColumnList.Add(new MandatoryColumns() { colIndex = j, columnName = "position" });
                        }
                        else if (cell.ToString().ToLower().Contains("company"))
                        {
                            MColumnList.Add(new MandatoryColumns() { colIndex = j, columnName = "company" });
                        }
                        else if (cell.ToString().ToLower().Contains("group"))
                        {
                            MColumnList.Add(new MandatoryColumns() { colIndex = j, columnName = "eventgroupid" });
                        }
                        else if (cell.ToString().ToLower().Contains("deadline"))
                        {
                            MColumnList.Add(new MandatoryColumns() { colIndex = j, columnName = "deadline" });
                        }
                        /* if header column is blank move to next*/
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                       // sb.Append("<th>" + cell.ToString() + "</th>");
                    }

                    //error message initialisation
                    ViewData["Import Error"] = "<ul>";

                    if (!MColumnList.Any(i => i.columnName == "email"))
                    {
                        ViewData["Import Error"] = ViewData["Import Error"] + "<li>No Email Address Column</li>";
                    }
                    else if (!MColumnList.Any(i => i.columnName == "fname"))
                    {
                        ViewData["Import Error"] = ViewData["Import Error"] + "<li>No First Name Column</li>";
                    }
                    else if (!MColumnList.Any(i => i.columnName == "lname"))
                    {
                        splitcol = true;
                       // ViewData["Import Error"] = ViewData["Import Error"] + "<li>No Last Name Column</li>";
                    }

                    ViewData["Import Error"] = ViewData["Import Error"]+ "</ul>";

                    //if manadatory column missing then return with error message
                    if ((string)ViewData["Import Error"] != "<ul></ul>")
                    {
                        return View();
                    }
                    sb.Append("</tr>");

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;

                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        sb.AppendLine(ValidRow(row, cellCount, MColumnList, splitcol));

                        //sb.AppendLine("</tr>");
                    }
                    sb.Append("</table>");
                }
            }

            // return this.Content(sb.ToString());
            return Json(Content(sb.ToString()));
        }

        private string ValidRow(IRow row, int cellCount, IList<MandatoryColumns> MColumnList, bool splitcol)
        {
            StringBuilder sb = new StringBuilder();
            
            var flag=false ;
            
            var record = new ColumnsToDB();

            foreach (var a in MColumnList)
            {   
                if (a.columnName == "email")
                {
                    
                    if (row.GetCell(a.colIndex) != null || row.GetCell(a.colIndex).ToString().Trim() != "")
                    {
                        var test = Validator.EmailIsValid(row.GetCell(a.colIndex).ToString().ToLower().Trim());

                        if (test==false)
                        {
                            sb.Append("<tr><td>" + row.RowNum + "</td>><td>" + row.GetCell(a.colIndex).ToString() + "</td><td>Not a Valid Email</td></tr>");
                            flag = true;
                        }
                    }
                    else
                    {
                        sb.Append("<tr><td>" + row.RowNum + "</td><td></td><td>Provide an email address</td></tr>");
                        flag = true;
                    }

                }
                else if (a.columnName == "fname")
                {
                    if (row.GetCell(a.colIndex) == null || row.GetCell(a.colIndex).ToString().Trim() == "")
                    {
                        sb.Append("<tr><td>" + row.RowNum + "</td><td><</td><td>First Name is missing</td></tr>");
                        flag = true;
                    }
                }
            }
            if (flag == false)//fname and email is valid
            {
                foreach (var a in MColumnList)
                {

                    if (splitcol == true && a.columnName == "fname")
                    {
                        var name = row.GetCell(a.colIndex).ToString().Trim().Split(" ", 2);
                        record.Fname = name[0];
                        record.Lname = name[1] ?? null;
                    }
                    if (a.columnName == "lname")
                    {
                        record.Lname = row.GetCell(a.colIndex).ToString().Trim() ?? null;
                    }
                    else if (a.columnName == "position")
                    {
                        record.Position = row.GetCell(a.colIndex).ToString().Trim() ?? null;
                    }
                    else if (a.columnName == "company")
                    {
                        record.Company = row.GetCell(a.colIndex).ToString().Trim() ?? null;
                    }
                    else if (a.columnName == "eventgroupid")
                    {
                        record.EventGroupId = row.GetCell(a.colIndex).ToString().Trim() ?? null;
                    }
                    else if (a.columnName == "deadline")
                    {
                        if (DateTime.TryParse(row.GetCell(a.colIndex).ToString().Trim(), out DateTime dateValue))
                        {
                            record.IndividualDeadline = dateValue;
                        }

                    }
                    //https://stackoverflow.com/questions/18081595/c-sharp-defining-hashset-with-custom-key
                    //https://dotnetcodr.com/2016/08/30/using-the-hashset-of-t-object-in-c-net-to-store-unique-elements-2/
                    //to remove duplicates hashset of objects

                    HashSet<ColumnsToDB> recordSet = new HashSet<ColumnsToDB>(new EmailComparer() )
                    {
                    record
                    };
                }
            }
            if (flag == true) { return sb.ToString(); } else { return ""; }
            
        }

        public sealed class EmailComparer : IEquatable<EmailComparer>
        {
            private readonly string yEmail;

            public EmailComparer(string email)
            {
                yEmail = email;
            }

            public bool Equals(EmailComparer x )
            {
                return x != null && x.yEmail.Equals(yEmail);
              //  return x.Email.Equals(y.Email, StringComparison.InvariantCultureIgnoreCase);
            }

            public override bool Equals(object obj)
            {
                return obj is ColumnsToDB && Equals((ColumnsToDB)obj);
            }

            public override int GetHashCode()
            {
                return yEmail.GetHashCode();
            }
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(_env.WebRootPath, fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

    }

    public static class Validator
    {

        static Regex ValidEmailRegex = CreateValidEmailRegex();

        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        internal static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }
    }

}
