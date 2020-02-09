using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Savaglow.Data;
using Savaglow.Data.Interfaces;
using savaglow_backend.Dtos;
using savaglow_backend.Helpers;
using Savaglow.Models.Ledger;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace savaglow_backend.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ILedgerRepository _ledgerRepo;
        public ReportsController(ILedgerRepository ledgerRepo)
        {
            this._ledgerRepo = ledgerRepo;

        }
        [HttpPost]
        public async Task<IActionResult> GetReport(string userId, ReportDto report)
        {
            var ledgerItems = await _ledgerRepo.GetLedgerForUser(userId, report.date);
            var recurringLedgerItems = await _ledgerRepo.GetRecurringLedgerForUser(userId, report.date);
            var ledgerIncome = ReportHelper.AddLedgerItems(ledgerItems, TransactionType.INCOME);
            var ledgerExpenses = ReportHelper.AddLedgerItems(ledgerItems, TransactionType.EXPENSE);
            var recurringIncome = ReportHelper.AddLedgerItems(recurringLedgerItems, TransactionType.INCOME);
            var recurringExpenses = ReportHelper.AddLedgerItems(recurringLedgerItems, TransactionType.EXPENSE);

            {

                // open xml sdk - docx
                using (MemoryStream mem = new MemoryStream())
                {
                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(mem, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                    {
                        wordDoc.AddMainDocumentPart();
                        // siga a ordem
                        Document doc = new Document();
                        Body body = new Body();

                        // 1 paragrafo
                        Paragraph para = new Paragraph();

                        ParagraphProperties paragraphProperties1 = new ParagraphProperties();
                        ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "Normal" };
                        Justification justification1 = new Justification() { Val = JustificationValues.Center };
                        ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();

                        paragraphProperties1.Append(paragraphStyleId1);
                        paragraphProperties1.Append(justification1);
                        paragraphProperties1.Append(paragraphMarkRunProperties1);

                        Run run = new Run();
                        RunProperties runProperties1 = new RunProperties();

                        Text text = new Text() { Text = "Ledger Report for " + report.date.ToString("MMMM yyyy") };

                        // siga a ordem 
                        run.Append(runProperties1);
                        run.Append(text);
                        para.Append(paragraphProperties1);
                        para.Append(run);

                        // 2 paragrafo
                        Paragraph para2 = new Paragraph();

                        ParagraphProperties paragraphProperties2 = new ParagraphProperties();
                        ParagraphStyleId paragraphStyleId2 = new ParagraphStyleId() { Val = "Normal" };
                        Justification justification2 = new Justification() { Val = JustificationValues.Start };
                        ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();

                        paragraphProperties2.Append(paragraphStyleId2);
                        paragraphProperties2.Append(justification2);
                        paragraphProperties2.Append(paragraphMarkRunProperties2);

                        Run incomeRunTitle = new Run();
                        Run expenseRunTitle = new Run();
                        Run recurringIncomeRunTitle = new Run();
                        Run recurringExpenseRunTitle = new Run();
                        RunProperties runProperties3 = new RunProperties();
                        Text text2 = new Text();
                        text2.Text = "Teste aqui";

                        var incomeRun = ReportHelper.DisplayItems(ledgerItems, TransactionType.INCOME);
                        var expenseRun = ReportHelper.DisplayItems(ledgerItems, TransactionType.EXPENSE);
                        var recurringIncomeRun = ReportHelper.DisplayRecurringItems(recurringLedgerItems, TransactionType.INCOME);
                        var recurringExpenseRun = ReportHelper.DisplayRecurringItems(recurringLedgerItems, TransactionType.EXPENSE);

                        incomeRunTitle.AppendChild(new Break());
                        incomeRunTitle.AppendChild(new Text("Income: " + "$" + ledgerIncome.ToString("0.##")));
                        incomeRunTitle.AppendChild(new Break());
                        incomeRunTitle.AppendChild(new Text("--------------------------------"));
                        incomeRunTitle.AppendChild(new Break());

                        expenseRunTitle.AppendChild(new Break());
                        expenseRunTitle.AppendChild(new Text("Expenses: " + "$" + ledgerExpenses.ToString("0.##")));
                        expenseRunTitle.AppendChild(new Break());
                        expenseRunTitle.AppendChild(new Text("--------------------------------"));
                        expenseRunTitle.AppendChild(new Break());

                        recurringIncomeRunTitle.AppendChild(new Break());
                        recurringIncomeRunTitle.AppendChild(new Text("Recurring Income: " + "$" + recurringIncome.ToString("0.##")));
                        recurringIncomeRunTitle.AppendChild(new Break());
                        recurringIncomeRunTitle.AppendChild(new Text("--------------------------------"));
                        recurringIncomeRunTitle.AppendChild(new Break());

                        recurringExpenseRunTitle.AppendChild(new Break());
                        recurringExpenseRunTitle.AppendChild(new Text("Recurring Expenses: " + "$" + recurringExpenses.ToString("0.##")));
                        recurringExpenseRunTitle.AppendChild(new Break());
                        recurringExpenseRunTitle.AppendChild(new Text("--------------------------------"));
                        recurringExpenseRunTitle.AppendChild(new Break());
                        para2.Append(incomeRunTitle);
                        para2.Append(incomeRun);
                        para2.Append(expenseRunTitle);
                        para2.Append(expenseRun);
                        para2.Append(recurringIncomeRunTitle);
                        para2.Append(recurringIncomeRun);
                        para2.Append(recurringExpenseRunTitle);
                        para2.Append(recurringExpenseRun);


                        // todos os 2 paragrafos no main body
                        body.Append(para);
                        body.Append(para2);

                        doc.Append(body);

                        wordDoc.MainDocumentPart.Document = doc;

                        wordDoc.Close();
                    }
                    return File(mem.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ABC.docx");
                }
            }

        }
    }
}