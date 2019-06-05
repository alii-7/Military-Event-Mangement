using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tarbya.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using Tarbya.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Tarbya.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ViewResult Index(/*string searchString, string searchString2, string searchString3*/)
        {
            
            var students = db.Students.Include(s => s.educationalQualification).Include(s => s.faculty).Include(s => s.year);
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    students = students.Where(s => s.firstName.Contains(searchString)
            //                           || s.secondName.Contains(searchString));
            //}
            //if (!String.IsNullOrEmpty(searchString2))
            //{
            //    students = students.Where(s => s.socialSecurityNumber.Contains(searchString2));
            //}
            //if (!String.IsNullOrEmpty(searchString3))
            //{
            //    students = students.Where(s => s.phoneNumber.Contains(searchString3));
            //}
            return View(students.ToList());
        }

        [HttpPost]
        public ViewResult Index(SearchViewModel searchViewModel)
        {
            Session["searchString"] = searchViewModel;
            
            var students = db.Students.Include(s => s.educationalQualification).Include(s => s.faculty).Include(s => s.year);
            if (!String.IsNullOrEmpty(searchViewModel.searchString1))
            {
                students = students.Where(s => s.firstName.Contains(searchViewModel.searchString1)
                                       || s.secondName.Contains(searchViewModel.searchString1)) ;
            }
            else if (!String.IsNullOrEmpty(searchViewModel.searchString2))
            {
                students = students.Where(s => s.socialSecurityNumber.Contains(searchViewModel.searchString2));
            }
            else if (!String.IsNullOrEmpty(searchViewModel.searchString3))
            {
                students = students.Where(s => s.phoneNumber.Contains(searchViewModel.searchString3));
            }
            //return RedirectToRoute(searchViewModel.searchString1.ToString());
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            student.faculty = db.Faculties.Where(x => x.ID == student.facultyID).FirstOrDefault();
            student.year = db.Years.Where(x => x.ID == student.yearID).FirstOrDefault();
            student.educationalQualification = db.EducationalQualifications.Where(x => x.ID == student.educationalQualificationID).FirstOrDefault();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.educationalQualificationID = new SelectList(db.EducationalQualifications, "ID", "educationalQualificationName");
            ViewBag.facultyID = new SelectList(db.Faculties, "ID", "facultyName");
            ViewBag.yearID = new SelectList(db.Years, "ID", "yearStatusName");
            //IEnumerable<string> gender;
            //gender.i
            //List<string> gender = new List<string>();
            //gender.Add("Male");
            //gender.Add("Female");

            IList<SelectListItem> religions = new List<SelectListItem>();
            religions.Add(new SelectListItem { Value = "مسلم", Text = "مسلم" });
            religions.Add(new SelectListItem { Value = "مسيحي", Text = "مسيحي" });
            ViewBag.religions = religions;

            IList<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem { Value = "ذكر", Text = "ذكر" });
            genders.Add(new SelectListItem { Value = "انثي", Text = "انثي" });
            ViewBag.genders = genders;

            //List<string> block = new List<string>();
            //block.Add("blocked");
            //block.Add("active");

            IList<SelectListItem> blocked = new List<SelectListItem>();
            blocked.Add(new SelectListItem { Value = "active", Text = "Active" });
            blocked.Add(new SelectListItem { Value = "blocked", Text = "Block" });
            ViewBag.blocked = blocked;

            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if(student.imageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(student.imageFile.FileName);
                string extension = Path.GetExtension(student.imageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                student.image = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                student.imageFile.SaveAs(fileName);
            }
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            IList<SelectListItem> religions = new List<SelectListItem>();
            religions.Add(new SelectListItem { Value = "مسلم", Text = "مسلم" });
            religions.Add(new SelectListItem { Value = "مسيحي", Text = "مسيحي" });
            ViewBag.religions = religions;

            IList<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem { Value = "ذكر", Text = "ذكر" });
            genders.Add(new SelectListItem { Value = "انثي", Text = "انثي" });
            ViewBag.genders = genders;

            IList<SelectListItem> blocked = new List<SelectListItem>();
            blocked.Add(new SelectListItem { Value = "blocked", Text = "Block" });
            blocked.Add(new SelectListItem { Value = "active", Text = "Active" });
            ViewBag.blocked = blocked;

            ViewBag.educationalQualificationID = new SelectList(db.EducationalQualifications, "ID", "educationalQualificationName", student.educationalQualificationID);
            ViewBag.facultyID = new SelectList(db.Faculties, "ID", "facultyName", student.facultyID);
            ViewBag.yearID = new SelectList(db.Years, "ID", "yearStatusName", student.yearID);

            ModelState.Clear();
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            IList<SelectListItem> religions = new List<SelectListItem>();
            religions.Add(new SelectListItem { Value = "مسلم", Text = "مسلم" });
            religions.Add(new SelectListItem { Value = "مسيحي", Text = "مسيحي" });
            ViewBag.religions = religions;

            IList<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem { Value = "ذكر", Text = "ذكر" });
            genders.Add(new SelectListItem { Value = "انثي", Text = "انثي" });
            ViewBag.genders = genders;

            IList<SelectListItem> blocked = new List<SelectListItem>();
            blocked.Add(new SelectListItem { Value = "active", Text = "Active" });
            blocked.Add(new SelectListItem { Value = "blocked", Text = "Block" });
            ViewBag.blocked = blocked;

            ViewBag.educationalQualificationID = new SelectList(db.EducationalQualifications, "ID", "educationalQualificationName", student.educationalQualificationID);
            ViewBag.facultyID = new SelectList(db.Faculties, "ID", "facultyName", student.facultyID);
            ViewBag.yearID = new SelectList(db.Years, "ID", "yearStatusName", student.yearID);

            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            IList<SelectListItem> religions = new List<SelectListItem>();
            religions.Add(new SelectListItem { Value = "مسلم", Text = "مسلم" });
            religions.Add(new SelectListItem { Value = "مسيحي", Text = "مسيحي" });
            ViewBag.religions = religions;

            IList<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem { Value = "ذكر", Text = "ذكر" });
            genders.Add(new SelectListItem { Value = "انثي", Text = "انثي" });
            ViewBag.genders = genders;

            IList<SelectListItem> blocked = new List<SelectListItem>();
            blocked.Add(new SelectListItem { Value = "active", Text = "Active" });
            blocked.Add(new SelectListItem { Value = "blocked", Text = "Block" });
            ViewBag.blocked = blocked;

            ViewBag.educationalQualificationID = new SelectList(db.EducationalQualifications, "ID", "educationalQualificationName", student.educationalQualificationID);
            ViewBag.facultyID = new SelectList(db.Faculties, "ID", "facultyName", student.facultyID);
            ViewBag.yearID = new SelectList(db.Years, "ID", "yearStatusName", student.yearID);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            student.faculty = db.Faculties.Where(x => x.ID == student.facultyID).FirstOrDefault();
            student.year = db.Years.Where(x => x.ID == student.yearID).FirstOrDefault();
            student.educationalQualification = db.EducationalQualifications.Where(x => x.ID == student.educationalQualificationID).FirstOrDefault();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
            var students = db.Students.Include(s => s.educationalQualification).Include(s => s.faculty).Include(s => s.year).ToList();
            if(Session["searchString"] != null)
            {
                SearchViewModel x = (SearchViewModel)Session["searchString"];

                if (!String.IsNullOrEmpty(x.searchString1))
                {
                    students = students.Where(s => s.firstName.Contains(x.searchString1)
                                           || s.secondName.Contains(x.searchString1)).ToList();
                }
                else if (!String.IsNullOrEmpty(x.searchString2))
                {
                    students = students.Where(s => s.socialSecurityNumber.Contains(x.searchString2)).ToList();
                }
                else if (!String.IsNullOrEmpty(x.searchString3))
                {
                    students = students.Where(s => s.phoneNumber.Contains(x.searchString3)).ToList();
                }
            }


            //First you create a Document and a PdfWriter and open the Document.
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Adding double border
            var content = pdfWriter.DirectContent;
            var pageBorderRect = new iTextSharp.text.Rectangle(pdfDoc.PageSize);
            var pageBorderRect2 = new iTextSharp.text.Rectangle(pdfDoc.PageSize);

            pageBorderRect.Left += pdfDoc.LeftMargin;
            pageBorderRect.Right -= pdfDoc.RightMargin;
            pageBorderRect.Top -= pdfDoc.TopMargin;
            pageBorderRect.Bottom += pdfDoc.BottomMargin;

            pageBorderRect2.Left += pdfDoc.LeftMargin - 5;
            pageBorderRect2.Right -= pdfDoc.RightMargin - 5;
            pageBorderRect2.Top -= pdfDoc.TopMargin - 5;
            pageBorderRect2.Bottom += pdfDoc.BottomMargin - 5;

            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Rectangle(pageBorderRect2.Left, pageBorderRect2.Bottom, pageBorderRect2.Width, pageBorderRect2.Height);
            content.Stroke();


            //ColumnText.ShowTextAligned(pdfWriter.DirectContent,Element.ALIGN_CENTER, new Phrase("asdasd علي"), 421, 545, 0,PdfWriter.RUN_DIRECTION_RTL,0);

            BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\Arial.ttf", BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font fontt = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.UNDERLINE);
            iTextSharp.text.Font fontt2 = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

            ColumnText column = new ColumnText(pdfWriter.DirectContent);
            ColumnText column2 = new ColumnText(pdfWriter.DirectContent);

            column.SetSimpleColumn(0, 520, 470, 570);
            column.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            column.AddElement(new Paragraph("كشف اسماء طلبة جامعة عين شمس", fontt));
            column.Go();


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

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

            iTextSharp.text.pdf.PdfPCell name = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "الاسم", font));
            iTextSharp.text.pdf.PdfPCell ssn = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "الرقم القومي", font));
            iTextSharp.text.pdf.PdfPCell phone = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "التليفون", font));
            iTextSharp.text.pdf.PdfPCell address = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "العنوان", font));
            iTextSharp.text.pdf.PdfPCell faculty = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, "الكلية", font));

            name.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            ssn.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            phone.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            address.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            faculty.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

            table.AddCell(phone);
            table.AddCell(ssn);
            table.AddCell(faculty);
            table.AddCell(address);
            table.AddCell(name);

            for (int i = 0; i < students.Count; i++)
            {
                string cellText5 = students[i].phoneNumber;
                string cellText4 = students[i].socialSecurityNumber;
                string cellText6 = students[i].faculty.facultyName;
                string cellText3 = students[i].address;
                string cellText2 = students[i].firstName + " " + students[i].secondName + " " + students[i].thirdName + " " + students[i].fourthName;

                iTextSharp.text.pdf.PdfPCell cell5 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText5, font));
                iTextSharp.text.pdf.PdfPCell cell4 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText4, font));
                iTextSharp.text.pdf.PdfPCell cell6 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText6, font));
                iTextSharp.text.pdf.PdfPCell cell3 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText3, font));
                iTextSharp.text.pdf.PdfPCell cell2 = new iTextSharp.text.pdf.PdfPCell(new Phrase(12, cellText2, font));


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

            table.WriteSelectedRows(0, -1, 30, 515, pcb);
            //pdfDoc.Add(table);


            //Chunk c1 = new Chunk("A chunk represents an isolated string. ");
            //font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            //Chunk c2 = new Chunk("علي صبري",font);


            //column.SetSimpleColumn(30, 30, 810, 515);
            //column.AddElement(table);
            //column.Go();



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
