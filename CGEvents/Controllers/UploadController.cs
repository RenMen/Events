using CGEvents.Models;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CGEvents.Controllers
{

    public class UploadController : Controller
    {
        private IHostingEnvironment _env;
        private readonly MiscFormsContext _context;
        public Regex regex = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
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
            public short EventGroupId;
            public short EventId;
            public DateTime IndividualDeadline;
        }

        public UploadController(MiscFormsContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }


        // GET: Upload
        public IActionResult Index(short? eid)
        {
            //var miscFormsContext = _context.Ams.Include(a => a.EventIdNavigation);
            //return View(await miscFormsContext.ToListAsync());
            if (eid != null)
            {
                ViewData["EventId"] = eid;
                ViewData["EventName"] = GetEventName(eid);
            }
            else {
                ViewData["EventId"] = null;
            }

            return View();
        }

        public string GetEventName(short? eid)
        {
            return _context.EventMaster.FirstOrDefault(id => id.EventId == eid).EventName;
        }
        public class EventDropDownModel
        {
            public string EventName { get; set; }

            public int EventId { get; set; }
        }

        //Get all upcoming events
        public ActionResult GetEvents()
        {
            var EventsDropDownList = _context.EventMaster.Where(id=> id.EventDateTo >= DateTime.Today).Select(e => new EventDropDownModel
            {
                EventName = e.EventName,
                EventId = e.EventId
            });

            return Json(EventsDropDownList);
            //    products = products.Where(p => p.ProductName.Contains(text));

            //return Json(products, JsonRequestBehavior.AllowGet);
        }
        // GET: Upload/Details/5
        public ActionResult Async_Save(IEnumerable<IFormFile> files, short? eid)
        {

            StringBuilder sb = new StringBuilder();

            if (eid != null)
            {

                IFormFile file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _env.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                short eGrpId = GetNextGroupID(eid) == null ? (short)1 : (short)(GetNextGroupID(eid) + 1);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    bool splitcol = false; //if file has no lname column then trigger the module to split the fname based on space character and create last name column
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
                            else if (cell.ToString().ToLower().Contains("company") || cell.ToString().ToLower().Contains("agency"))
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

                        ViewData["Import Error"] = ViewData["Import Error"] + "</ul>";

                        //if manadatory column missing then return with error message
                        if ((string)ViewData["Import Error"] != "<ul></ul>")
                        {
                            return View();
                        }


                        var errorTD = ""; //this variable will have the html to display error if the row is not valid

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {

                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;

                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            errorTD = errorTD + ValidRow(row, cellCount, MColumnList, splitcol);

                            //sb.AppendLine("</tr>");
                        }

                        if (errorTD == "") // if no error loop thru xl file and  create hashset to write to database
                        {
                            // HashSet<ColumnsToDB> recordSet = new HashSet<ColumnsToDB>(new EmailComparer());
                                                       
                            HashSet<Ams> recordSet = new HashSet<Ams>(new EmailComparer1());
                            IList<Ams> recordSetL = new List<Ams>();
                            StringBuilder tranferTD = new StringBuilder();
                            var checks = _context.Ams.Where(i => i.EventId == eid).Select(r => r.EmailId).ToArray();
                            var emailExistInDb = 0;

                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                            {

                                IRow row = sheet.GetRow(i);
                                var r = new ColumnsToDB();
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                                r= GetHashSet(row, cellCount, MColumnList, splitcol);

                                if (!checks.Contains(r.Email))
                                {
                                  recordSet.Add(new Ams { Fname = r.Fname, Lname = r.Lname, EmailId = r.Email, Position = r.Position, Company = r.Company ,EventId=eid,EventGroupId=eGrpId});
                                    if (r.Fname.Length <= 2 || r.Fname.Contains(".") || r.Fname.Contains("_") || r.Fname.Contains("-"))
                                    {
                                        tranferTD.Append("<tr><td  class='btn-primary'>" + r.Fname + "</td><td  class='btn-primary'>" + r.Lname + "</td><td  class='btn-primary'>" + r.Email + "</td></tr>");
                                    }
                                    else
                                    {
                                        tranferTD.Append("<tr><td>" + r.Fname + "</td><td>" + r.Lname + "</td><td>" + r.Email + "</td></tr>");
                                    }
                                }
                                else
                                {
                                    emailExistInDb = emailExistInDb + 1;
                                }


                                //****
                                // check recordSet for fname =="Error occured" string and report to user to contact marketing department
                                //*****

                            }
                            //Bulkinsert require a list. to remove duplicates we use hashset. so converted hashset to list
                             recordSetL= recordSet.ToList();
                            //follow the below link to bulk insert - to improve insertion time
                            //https://janaks.com.np/bulk-insert-in-entityframework-core/ 
                            //https://github.com/borisdj/EFCore.BulkExtensions
                            _context.BulkInsert(recordSetL);
                            
                            sb.Append("<div class='alert alert-success' role='success'> <h4 class='alert-heading'>" + (recordSet.Count == 0 ? "No Records Updated.!": "Successfully Updated.!") + "</h4></div>" +
                            "<ul><li>(" + recordSet.Count().ToString() + ") Records Inserted </li> <li>(" + (sheet.LastRowNum - recordSet.Count- emailExistInDb).ToString() + ") Duplicates found and eliminated</li><li>(" + (emailExistInDb).ToString() + ") Already Exist in Database</li></ul>");
                            sb.Append("<table id='resultGrid' class='table table-striped table-bordered'>");
                            sb.Append("<th>First Name</th><th>Last Name</th><th>Email</th>");
                            sb.Append((tranferTD.ToString()=="" ? "<tr><td colspan='3'>No Records to update</td></tr>":tranferTD.ToString()) + "</table>");
                            return Json(Content(sb.ToString()));
                        }
                        else
                        {
                            sb.Append("<div class='alert alert-danger' role='alert'> <h4 class='alert-heading'> File not updated.!</h4><p> Please fix the below errors in" + file.FileName + "and try upload again</p></div>");
                            sb.Append("<table id='resultGrid' class='table table-striped table-bordered'>");
                            sb.Append("<th>Row#</th><th>First Name/ Email</th><th>Error Message</th>");
                            sb.Append(errorTD + "</tr> </table>");
                            //Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                            return Json(Content(sb.ToString()));
                        }
                    }
                }
                else
                {
                    sb.Append("<div class='alert alert-danger' role='alert'> <h4 class='alert-heading'>No files selected !</h4></div>");
                    return Json(Content(sb.ToString()));
                }
            }
            else
            {
                //Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                sb.Append("<div class='alert alert-danger' role='alert'> <h4 class='alert-heading'>No Event Selected !</h4><p> Please select an event from the drop down list</p></div>");
                return Json(Content(sb.ToString()));
            }

            // return this.Content(sb.ToString());

        }
        public short? GetNextGroupID(short? eid)
        {
            return _context.Ams.Where(w => w.EventId == eid).Select(p => p.EventGroupId).Max();
        }
        private string ValidRow(IRow row, int cellCount, IList<MandatoryColumns> MColumnList, bool splitcol)
        {
            StringBuilder sb = new StringBuilder();

            var flag = false;

            var record = new ColumnsToDB();

            foreach (var a in MColumnList)
            {
                if (a.columnName == "email")
                {

                    if (row.GetCell(a.colIndex) != null || row.GetCell(a.colIndex).ToString().Trim() != "")
                    {
                        var test = Validator.EmailIsValid(row.GetCell(a.colIndex).ToString().ToLower().Trim());

                        if (test == false)
                        {
                            sb.Append("<tr><td>" + row.RowNum + "</td><td>" + row.GetCell(a.colIndex).ToString() + "</td><td>Not a Valid Email</td></tr>");
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
                    //Remove special characters from name
                    var fname = (row.GetCell(a.colIndex) == null) ? null : regex.Replace(row.GetCell(a.colIndex).ToString(), String.Empty);
                    if (fname == null || fname.ToString().Trim() == "")
                    {
                        sb.Append("<tr><td>" + row.RowNum + "</td><td></td><td>First Name is missing</td></tr>");
                        flag = true;
                    }
                }
            }

            if (flag == true) { return sb.ToString(); } else { return ""; }

        }



        public ColumnsToDB GetHashSet(IRow row, int cellCount, IList<MandatoryColumns> MColumnList, bool splitcol)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var record = new ColumnsToDB();
            try
            {

                foreach (var a in MColumnList)
                {

                    if (splitcol == true && a.columnName == "fname")
                    {
                        var name = textInfo.ToTitleCase(row.GetCell(a.colIndex).ToString().ToLower().Trim()).Split(" ", 2);
                        //if any change in expression change in validRow module as well
                        name[0] = regex.Replace(name[0], String.Empty); //remove any special character
                        record.Fname = name[0];
                        record.Lname = (name.Length > 0) ? null : textInfo.ToTitleCase(name[1].ToString().ToLower().Trim());
                    }
                    else if (splitcol == false && a.columnName == "fname")
                    {
                        record.Fname = textInfo.ToTitleCase(row.GetCell(a.colIndex).ToString().ToLower().Trim());

                    }
                    if (a.columnName == "lname")
                    {
                        record.Lname = row.GetCell(a.colIndex) == null ? null : textInfo.ToTitleCase(row.GetCell(a.colIndex).ToString().ToLower().Trim());
                    }
                    else if (a.columnName == "email")
                    {
                        record.Email = row.GetCell(a.colIndex).ToString().ToLower().Trim();
                    }
                    else if (a.columnName == "position")
                    {
                        record.Position = row.GetCell(a.colIndex) == null ? null : textInfo.ToTitleCase(row.GetCell(a.colIndex).ToString().ToLower().Trim());
                    }
                    else if (a.columnName == "company")
                    {
                        record.Company = row.GetCell(a.colIndex) == null ? null : textInfo.ToTitleCase(row.GetCell(a.colIndex).ToString().ToLower().Trim());
                    }
                    else if (a.columnName == "eventgroupid")
                    {
                        if (short.TryParse(row.GetCell(a.colIndex).ToString(), out short gid))
                        {
                            record.EventGroupId = gid;
                        }
                    }
                    else if (a.columnName == "deadline")
                    {
                        if (DateTime.TryParse(row.GetCell(a.colIndex).ToString().Trim(), out DateTime dateValue))
                        {
                            record.IndividualDeadline = dateValue;
                        }

                    }
                }; //record created

                //https://stackoverflow.com/questions/18081595/c-sharp-defining-hashset-with-custom-key
                //https://dotnetcodr.com/2016/08/30/using-the-hashset-of-t-object-in-c-net-to-store-unique-elements-2/
                //to remove duplicates hashset of objects
                return record;
            }
            catch (Exception ex)
            {
                Console.Write(ex.InnerException.Message);
                record.Fname = "Error occured";
                record.Email = row.RowNum + "@rownumber";
                return record;
                throw;
            }

        }

        public sealed class EmailComparer : IEqualityComparer<ColumnsToDB>
        {
            public bool Equals(ColumnsToDB x, ColumnsToDB y)
            {
                return x.Email.Equals(y.Email, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(ColumnsToDB obj)
            {
                return obj.Email.GetHashCode();
            }
        }

        public sealed class EmailComparer1 : IEqualityComparer<Ams>
        {
            public bool Equals(Ams x, Ams y)
            {
                return x.EmailId.Equals(y.EmailId, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(Ams obj)
            {
                return obj.EmailId.GetHashCode();
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
