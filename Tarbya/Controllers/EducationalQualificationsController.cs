using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tarbya.Models;

namespace Tarbya.Controllers
{
    [Authorize]
    public class EducationalQualificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EducationalQualifications
        public ActionResult Index()
        {
            return View(db.EducationalQualifications.ToList());
        }

        // GET: EducationalQualifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationalQualification educationalQualification = db.EducationalQualifications.Find(id);
            if (educationalQualification == null)
            {
                return HttpNotFound();
            }
            return View(educationalQualification);
        }

        // GET: EducationalQualifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EducationalQualifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,educationalQualificationName")] EducationalQualification educationalQualification)
        {
            if (ModelState.IsValid)
            {
                db.EducationalQualifications.Add(educationalQualification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(educationalQualification);
        }

        // GET: EducationalQualifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationalQualification educationalQualification = db.EducationalQualifications.Find(id);
            if (educationalQualification == null)
            {
                return HttpNotFound();
            }
            return View(educationalQualification);
        }

        // POST: EducationalQualifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,educationalQualificationName")] EducationalQualification educationalQualification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(educationalQualification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(educationalQualification);
        }

        // GET: EducationalQualifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationalQualification educationalQualification = db.EducationalQualifications.Find(id);
            if (educationalQualification == null)
            {
                return HttpNotFound();
            }
            return View(educationalQualification);
        }

        // POST: EducationalQualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EducationalQualification educationalQualification = db.EducationalQualifications.Find(id);
            db.EducationalQualifications.Remove(educationalQualification);
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
    }
}
