using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tarbya.Models;
using Tarbya.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;

namespace Tarbya.Controllers
{
    [Authorize]
    public class StudentEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //this action is to asssign students to events 
        // GET: StudentEvents
        public ActionResult Index()
        {
            ViewBag.eventID = new SelectList(db.Events, "ID", "eventName");

            IList<SelectListItem> studentList = db.Students.Where(s => s.Blocked.Contains("active")).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.firstName + " " + x.secondName + " " + x.thirdName + " " + x.fourthName + " - " + x.phoneNumber + " - " + x.socialSecurityNumber }).ToList();
            ViewBag.studentList = studentList;

            return View();
        }

        //this action is to asssign students to events   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(StudentEventViewModel studentEventViewModel)
        {
            List<StudentEvent> studentEvents = db.StudentEvents.ToList();
            if (ModelState.IsValid)
            {
                StudentEvent studentEvent = new StudentEvent();
                for (int i = 0; i < studentEventViewModel.students.Count; i++)
                {
                    studentEvent.eventID = studentEventViewModel.eventID;
                    studentEvent.studentID = studentEventViewModel.students[i] ?? 0;
                    bool contains = false;
                    for (int k = 0; k < studentEvents.Count; k++)
                    {
                        if (studentEvents[k].eventID == studentEvent.eventID && studentEvents[k].student.ID == studentEvent.studentID)
                        {
                            contains = true;
                        }
                    }
                    if (!contains)
                    {
                        db.StudentEvents.Add(studentEvent);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Vieww");
            }

            ViewBag.eventID = new SelectList(db.Events, "ID", "eventName");
            IList<SelectListItem> studentList = db.Students.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.firstName + " " + x.secondName }).ToList();
            ViewBag.studentList = studentList;

            return View();
        }

        public ViewResult Vieww()
        {

            var list = db.StudentEvents.Include(s => s.student).Include(s => s.eventt).OrderBy(s => s.eventt.eventName);

            return View(list.ToList());
        }

        [HttpPost]
        public ViewResult Vieww(SearchViewModel searchViewModel)
        {
            Session["eventSearchString"] = searchViewModel;

            var students = db.StudentEvents.Include(s => s.student).Include(s => s.eventt);
            if (!String.IsNullOrEmpty(searchViewModel.searchString1))
            {
                students = students.Where(s => s.eventt.eventName.Contains(searchViewModel.searchString1));
            }

            //return RedirectToRoute(searchViewModel.searchString1.ToString());
            return View(students.ToList());
        }


        // GET: StudentEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentEvent studentEvent = db.StudentEvents.Find(id);
            if (studentEvent == null)
            {
                return HttpNotFound();
            }
            return View(studentEvent);
        }

        // GET: StudentEvents/Create
        public ActionResult Create()
        {
            ViewBag.eventID = new SelectList(db.Events, "ID", "eventName");
            ViewBag.studentID = new SelectList(db.Students, "ID", "firstName");
            return View();
        }

        // POST: StudentEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,studentID,eventID")] StudentEvent studentEvent)
        {
            if (ModelState.IsValid)
            {
                db.StudentEvents.Add(studentEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.eventID = new SelectList(db.Events, "ID", "eventName", studentEvent.eventID);
            ViewBag.studentID = new SelectList(db.Students, "ID", "firstName", studentEvent.studentID);
            return View(studentEvent);
        }

        // GET: StudentEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentEvent studentEvent = db.StudentEvents.Find(id);
            if (studentEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.eventID = new SelectList(db.Events, "ID", "eventName", studentEvent.eventID);
            ViewBag.studentID = new SelectList(db.Students, "ID", "firstName", studentEvent.studentID);
            return View(studentEvent);
        }

        // POST: StudentEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,studentID,eventID")] StudentEvent studentEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.eventID = new SelectList(db.Events, "ID", "eventName", studentEvent.eventID);
            ViewBag.studentID = new SelectList(db.Students, "ID", "firstName", studentEvent.studentID);
            return View(studentEvent);
        }

        // GET: StudentEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentEvent studentEvent = db.StudentEvents.Find(id);
            if (studentEvent == null)
            {
                return HttpNotFound();
            }
            return View(studentEvent);
        }

        // POST: StudentEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentEvent studentEvent = db.StudentEvents.Find(id);
            db.StudentEvents.Remove(studentEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Print()
        {
            var students = db.StudentEvents.Include(s => s.student).Include(s => s.student.faculty).Include(s => s.eventt).ToList();
            if (Session["eventSearchString"] != null)
            {
                SearchViewModel x = (SearchViewModel)Session["eventSearchString"];

                if (!String.IsNullOrEmpty(x.searchString1))
                {
                    students = students.Where(s => s.eventt.eventName.Contains(x.searchString1)).ToList();
                }
            }

            //First you create a Document and a PdfWriter and open the Document.
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Adding double border
            var content = pdfWriter.DirectContent;
            var pageBorderRect = new Rectangle(pdfDoc.PageSize);
            var pageBorderRect2 = new Rectangle(pdfDoc.PageSize);

            pageBorderRect.Left += pdfDoc.LeftMargin;
            pageBorderRect.Right -= pdfDoc.RightMargin;
            pageBorderRect.Top -= pdfDoc.TopMargin;
            pageBorderRect.Bottom += pdfDoc.BottomMargin;

            pageBorderRect2.Left += pdfDoc.LeftMargin-5;
            pageBorderRect2.Right -= pdfDoc.RightMargin-5;
            pageBorderRect2.Top -= pdfDoc.TopMargin - 5;
            pageBorderRect2.Bottom += pdfDoc.BottomMargin - 5;

            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Rectangle(pageBorderRect2.Left, pageBorderRect2.Bottom, pageBorderRect2.Width, pageBorderRect2.Height);
            content.Stroke();


            //ColumnText.ShowTextAligned(pdfWriter.DirectContent,Element.ALIGN_CENTER, new Phrase("asdasd علي"), 421, 545, 0,PdfWriter.RUN_DIRECTION_RTL,0);

            BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\Arial.ttf", BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font fontt = new iTextSharp.text.Font(bf, 13, iTextSharp.text.Font.UNDERLINE);
            iTextSharp.text.Font fontt2 = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

            ColumnText column = new ColumnText(pdfWriter.DirectContent);
            ColumnText column2 = new ColumnText(pdfWriter.DirectContent);

            column.SetSimpleColumn(0, 520, 500, 570);
            column.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            column.AddElement(new Paragraph("كشف اسماء طلبة جامعة عين شمس", fontt));
            column.Go();

            column.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            column.AddElement(new Paragraph("الحاضرون " + students[0].eventt.eventName, fontt));
            column.Go();

            fontt = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.UNDERLINE);

            column.SetSimpleColumn(0, 520, 810, 570);
            column.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            column.AddElement(new Paragraph("قيادة قوات الدفاع الشعبي و العسكري", fontt));
            column.AddElement(new Paragraph("ادارة التربيه العسكريه", fontt));
            string dateNow = DateTime.Now.ToString("dd dddd , MMMM, yyyy", new CultureInfo("ar-AE"));
            column.AddElement(new Paragraph(dateNow, fontt2));
            column.Go();

            column.SetSimpleColumn(0, 0, 200, 80);
            column.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            column.AddElement(new Paragraph("التوقيع (                        ) ", fontt2));
            column.AddElement(new Paragraph("مقدم / مصطفي عبدالله", fontt2));
            column.AddElement(new Paragraph("مدير ادارة التربية العسكرية", fontt2));
            column.Go();

            ////Table
            PdfContentByte pcb = pdfWriter.DirectContent;
            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 782f;

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);

            iTextSharp.text.pdf.PdfPCell name = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "الاسم", font));
            iTextSharp.text.pdf.PdfPCell faculty = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "الكلية", font));
            iTextSharp.text.pdf.PdfPCell ssn = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "الرقم القومي", font));
            iTextSharp.text.pdf.PdfPCell phone = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "التليفون", font));
            iTextSharp.text.pdf.PdfPCell address = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "العنوان", font));

            name.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            faculty.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            ssn.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            address.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

            table.AddCell(phone);
            table.AddCell(ssn);
            table.AddCell(faculty);
            table.AddCell(address);
            table.AddCell(name);

            for (int i = 0; i < students.Count; i++)
            {
                string cellText2 = students[i].student.firstName + " " + students[i].student.secondName + " " + students[i].student.thirdName + " " + students[i].student.fourthName;
                string cellText3 = students[i].student.address;
                string cellText4 = students[i].student.socialSecurityNumber;
                string cellText5 = students[i].student.phoneNumber;
                //string cellText6 = db.Faculties.Where(x => x.ID == students[i].student.facultyID).ToList()[0].facultyName;
                string cellText6 = students[i].student.faculty.facultyName;

                font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.pdf.PdfPCell cell2 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText2, font));
                iTextSharp.text.pdf.PdfPCell cell3 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText3, font));
                iTextSharp.text.pdf.PdfPCell cell4 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText4, font));
                iTextSharp.text.pdf.PdfPCell cell5 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText5, font));
                iTextSharp.text.pdf.PdfPCell cell6 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText6, font));


                cell2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell3.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell4.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell5.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell6.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                table.AddCell(cell5);
                table.AddCell(cell4);
                table.AddCell(cell6);
                table.AddCell(cell3);
                table.AddCell(cell2);
            }

            table.WriteSelectedRows(0, -1, 30, 515,pcb);

            //pdfDoc.Add(table);

            //To create a Pdf use the below code and place it on the end.
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StudentEventReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return View();
        }
    }
}
