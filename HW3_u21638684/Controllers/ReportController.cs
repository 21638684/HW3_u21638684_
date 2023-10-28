using HW3_u21638684.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using OfficeOpenXml;
using System.IO;

namespace HW3_u21638684.Controllers
{
    public class ReportController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // GET: Report
        public ActionResult Report()
        {
            var popularBooks = db.borrows
          .GroupBy(b => b.bookId)
          .Select(g => new ReportVM
          {
              BookId = g.Key.Value,
              BookName = g.FirstOrDefault().book.name,
              BorrowCount = g.Count()
          })
          .OrderByDescending(b => b.BorrowCount)
          .ToList();

            string reportsDirectory = Server.MapPath("~/Reports");
            string[] fileNames = Directory.GetFiles(reportsDirectory)
                                        .Select(Path.GetFileName)
                                        .ToArray();

            ViewBag.FileNames = fileNames;

            return View(popularBooks);
           
        }
        private const string SavedFilesDirectory = "~/Reports/";

        [HttpPost]
        public ActionResult SaveReport(string fileName, string fileType, List<ReportVM> reportData)
        {
            string filePath = Path.Combine(Server.MapPath("~/Reports"), $"{fileName}.{fileType}");

           
                if (fileType.ToLower() == "pdf")
                {
                 
                    var reportHtml = RenderRazorViewToString("Report", reportData); 
                    var pdfBytes = new Rotativa.ViewAsPdf("Report", reportData).BuildFile(this.ControllerContext);

                    System.IO.File.WriteAllBytes(filePath, pdfBytes);
                }
                else if (fileType.ToLower() == "xlsx")
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new OfficeOpenXml.ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Report");

                        worksheet.Cells[1, 1].Value = "Book Name";
                        worksheet.Cells[1, 2].Value = "Borrow Count";

                        int row = 2;
                        foreach (var item in reportData)
                        {
                            worksheet.Cells[row, 1].Value = item.BookName;
                            worksheet.Cells[row, 2].Value = item.BorrowCount;
                            row++;
                        }

                        byte[] fileBytes = package.GetAsByteArray();
                        System.IO.File.WriteAllBytes(filePath, fileBytes);
                    }
                }
                else
                {
                  
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Report");

        }

        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }

        }
        public ActionResult Downloadfile(string fileName)
        {
            string filePath = Path.Combine(Server.MapPath("~/Reports"), fileName);

            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {

                return RedirectToAction("Report");
            }
        }
        [HttpPost]
        public ActionResult Deletefile(string fileName)
        {
            string filePath = Path.Combine(Server.MapPath("~/Reports"), fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return RedirectToAction("Report");
            }
            else
            {

                return RedirectToAction("Report");
            }
        }

    }
}
