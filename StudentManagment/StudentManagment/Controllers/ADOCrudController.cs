using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManagment.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace StudentManagment.Controllers
{
    public class ADOCrudController : Controller
    {
        private ketanEntities db = new ketanEntities();
        private string ConnString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult Index()
        {
            var model = new List<StudentManagment.Data.Student>();
            try
            {
                using (var conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    var query = "select * from Student";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var obj = new Student();
                        obj.FirstName  = dataTable.Rows[i]["FirstName"].ToString();
                        obj.LastName = dataTable.Rows[i]["LastName"].ToString();
                        obj.StudentId = Convert.ToInt32(dataTable.Rows[i]["StudentId"]);
                        obj.Percent = Convert.ToInt32( dataTable.Rows[i]["Percent"]);
                        model.Add(obj);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }

        public ActionResult Details(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ADOCrud/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        public ActionResult Edit(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

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
            return View(student);
        }

        public ActionResult Delete(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


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
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}