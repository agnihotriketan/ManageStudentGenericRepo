using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManagment.Data;

namespace StudentManagment.Controllers
{
    public class StudentMarksController : Controller
    {
        private ketanEntities db = new ketanEntities();

        //
        // GET: /StudentMarks/

        public ActionResult Index()
        {
            var studentmarks = db.StudentMarks.Include(s => s.Student);
            return View(studentmarks.ToList());
        }

        //
        // GET: /StudentMarks/Details/5

        public ActionResult Details(int id = 0)
        {
            var studentmark = db.StudentMarks.Find(id);
            if (studentmark == null)
            {
                return HttpNotFound();
            }
            return View(studentmark);
        }

        //
        // GET: /StudentMarks/Create

        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName");
            return View();
        }

        //
        // POST: /StudentMarks/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentMark studentmark)
        {
            if (ModelState.IsValid)
            {
                db.StudentMarks.Add(studentmark);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", studentmark.StudentId);
            return View(studentmark);
        }

        //
        // GET: /StudentMarks/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var studentmark = db.StudentMarks.Find(id);
            if (studentmark == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", studentmark.StudentId);
            return View(studentmark);
        }

        //
        // POST: /StudentMarks/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentMark studentmark)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentmark).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FirstName", studentmark.StudentId);
            return View(studentmark);
        }

        //
        // GET: /StudentMarks/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var studentmark = db.StudentMarks.Find(id);
            if (studentmark == null)
            {
                return HttpNotFound();
            }
            return View(studentmark);
        }

        //
        // POST: /StudentMarks/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var studentmark = db.StudentMarks.Find(id);
            db.StudentMarks.Remove(studentmark);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}