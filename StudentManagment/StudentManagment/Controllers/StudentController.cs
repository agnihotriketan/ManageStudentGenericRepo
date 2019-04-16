using System.Web.Mvc;
using StudentManagment.Data;
using StudentManagment.GenericRepo;

namespace StudentManagment.Controllers
{
    public class StudentController : Controller
    {
        //private ketanEntities db = new ketanEntities();
        GenericRepository<Student> db = new GenericRepository<Student>(new ketanEntities());

        public ActionResult Index()
        {
            return View(db.GetAll());
        }

        public ActionResult Details(int id = 0)
        {
            var student = db.GetById(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Insert(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        public ActionResult Edit(int id = 0)
        {
            var student = db.GetById(id);
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
                db.Edit(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }


        public ActionResult Delete(int id = 0)
        {
            var student = db.GetById(id);

            if (student == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Delete(student);
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var student = db.GetById(id);
            db.Delete(student);
            return RedirectToAction("Index");
        }

        /*  protected override void Dispose(bool disposing)
          {
              db.Dispose();
              base.Dispose(disposing);
          }*/
    }
}