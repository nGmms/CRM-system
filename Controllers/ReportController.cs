using CRMSystem.Data;
using CRMSystem.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Controllers
{
    // Authorize attribute restricts access to the controller actions to users with the 'Manager' role.
    [Authorize(Roles = "Manager")]
    public class ReportController : Controller
    {
        // Field to hold an instance of the ApplicationDbContext.
        private readonly ApplicationDbContext _context;

       // Constructor that initializes the context field with the injected ApplicationDbContext.
        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action method for generating a report. This report includes details of customers and their associated calls.
        public async Task<IActionResult> GenerateReport()
        {
           // Querying the database to get customer and call data. This includes each customer and their respective calls.
            var customerCallReports = await _context.Customers
                .Select(c => new 
                {
                    Customer = c,
                    Calls = _context.Calls.Where(call => call.CustomerNo == c.CustomerNo).ToList()
                }).ToListAsync();

            // Using ClosedXML to create an Excel workbook.
            using (var workbook = new XLWorkbook())
            {
                // Adding a new worksheet to the workbook with a title.
                var worksheet = workbook.Worksheets.Add("CustomerCallsReport");

                int currentRow = 1;

                // Iterating over each customer and their calls to populate the worksheet.
                foreach (var report in customerCallReports)
                {
                    // Customer details
                    worksheet.Cell(currentRow, 1).Value = "Customer No";
                    worksheet.Cell(currentRow, 2).Value = "Customer Name";
                    worksheet.Cell(currentRow, 3).Value = report.Customer.CustomerNo;
                    worksheet.Cell(currentRow, 4).Value = report.Customer.Name;
                    currentRow += 2;

                    // Call details headers
                    worksheet.Cell(currentRow, 1).Value = "Call No";
                    worksheet.Cell(currentRow, 2).Value = "Date Of Call";
                    worksheet.Cell(currentRow, 3).Value = "Subject";
                    currentRow++;

                    // Call details
                    foreach (var call in report.Calls)
                    {
                        worksheet.Cell(currentRow, 1).Value = call.CallNo;
                        worksheet.Cell(currentRow, 2).Value = call.DateOfCall;
                        worksheet.Cell(currentRow, 3).Value = call.Subject;
                        currentRow++;
                    }

                    // Add a space between different customers
                    currentRow++;
                }

                // Using a MemoryStream to save and return the workbook as a file download.
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    // Returning the file to be downloaded by the user
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerCallsReport.xlsx");
                }
            }
        }
    }
}
