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
        private readonly string ConnString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

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
                        obj.FirstName = dataTable.Rows[i]["FirstName"].ToString();
                        obj.LastName = dataTable.Rows[i]["LastName"].ToString();
                        obj.StudentId = Convert.ToInt32(dataTable.Rows[i]["StudentId"]);
                        obj.Percent = Convert.ToInt32(dataTable.Rows[i]["Percent"]);
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
            Student student = new Student(); // db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            GetStudentById(id, student);
            return View(student);
        }

        private void GetStudentById(int id, Student student)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                var query = "select * from Student where StudentId=@StudentId";// +id;
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@StudentId", id);
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        student.StudentId = Convert.ToInt32(dr["StudentId"]);
                        student.FirstName = dr["FirstName"].ToString();
                        student.LastName = dr["LastName"].ToString();
                        student.Percent = Convert.ToDouble(dr["Percent"]);
                    }
                }
            }
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
                using (var conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    var query = "INSERT INTO [Student] ([FirstName],[LastName],[Percent])VALUES(@FirstName ,@LastName ,@Percent)";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.LastName);
                    cmd.Parameters.AddWithValue("@Percent", student.Percent);
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }

            return View(student);
        }

        public ActionResult Edit(int id = 0)
        {
            Student student = new Student();// db.Students.Find(id);
            GetStudentById(id, student);
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
                using (var conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    var query = "UPDATE [Student] SET [FirstName] = @FirstName,[LastName] = @LastName,[Percent] =100 WHERE StudentId=@StudentId";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
                    cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.LastName);
                    cmd.Parameters.AddWithValue("@Percent", student.Percent);
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult Delete(int id = 0)
        {
            Student student = new Student();
            GetStudentById(id, student);
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
            try
            {
                using (var conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    var query = "Delete From Student where StudentId=@StudentId";
                    var command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@StudentId", id);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}