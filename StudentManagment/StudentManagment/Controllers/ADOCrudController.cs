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
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Index()
        {
            var model = new List<Student>();
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    var query = "select * from Students";
                    var cmd = new SqlCommand(query, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    for (var i = 0; i < dataTable.Rows.Count; i++)
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
            var student = new Student(); // db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            // Execute Sp 
            /* using (var con= new SqlConnection(_connString))
             {
                 con.Open();
                 var command = new SqlCommand("GetStudentById", con);
                 command.CommandType = CommandType.StoredProcedure;
                 command.Parameters.Add("@StudentId", 2);
                var  adapter = new SqlDataAdapter(command);
                var dt = new DataTable();
                adapter.Fill(dt); 
             }*/

            //Execute Sp with out parameter
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                var comm = new SqlCommand("OutDemo", conn);
                comm.CommandType = CommandType.StoredProcedure;
                int x = 0;
                comm.Parameters.Add("@Count", SqlDbType.Int);
                comm.Parameters["@Count"].Direction = ParameterDirection.Output;
                comm.ExecuteNonQuery();
                ViewBag.Count = 0; 
                ViewBag.Count = comm.Parameters["@Count"].Value.ToString();
            }

            GetStudentById(id, student);
            return View(student);
        }

        private void GetStudentById(int id, Student student)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                const string query = "select * from Students where StudentId=@StudentId"; // +id;
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@StudentId", id);
                using (var dr = command.ExecuteReader())
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
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    const string query = "INSERT INTO [Students] ([FirstName],[LastName],[Percent])VALUES(@FirstName ,@LastName ,@Percent)";
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
            var student = new Student();// db.Students.Find(id);
            GetStudentById(id, student);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    const string query = "UPDATE [Students] SET [FirstName] = @FirstName,[LastName] = @LastName,[Percent] =@Percent WHERE StudentId=@StudentId";
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
            var student = new Student();
            GetStudentById(id, student);
            return View(student);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    const string query = "Delete From Students where StudentId=@StudentId";
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