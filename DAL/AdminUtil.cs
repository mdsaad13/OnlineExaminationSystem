using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.DAL
{
    public class AdminUtil
    {
        private readonly SqlConnection Conn = new SqlConnection($"Data Source=localhost;Initial Catalog={AppInfo.DbName};Integrated Security=True");

        internal List<ContactUs> AllEnquiries()
        {
            DataTable td = new DataTable();
            List<ContactUs> list = new List<ContactUs>();
            try
            {
                string sqlquery = "SELECT * FROM enquiry ORDER BY id DESC";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                Conn.Open();
                adp.Fill(td);
                foreach (DataRow row in td.Rows)
                {
                    ContactUs obj = new ContactUs
                    {
                        ID = Convert.ToInt32(row["id"]),
                        Name = Convert.ToString(row["name"]),
                        Email = Convert.ToString(row["email"]),
                        Mobile = Convert.ToString(row["mobile"]),
                        Sub = Convert.ToString(row["subject"]),
                        Message = Convert.ToString(row["message"]),
                        DateTime = Convert.ToDateTime(row["datetime"])
                    };

                    list.Add(obj);
                }
            }
            catch (Exception)
            { }
            finally
            {
                Conn.Close();
            }
            return list;
        }

        internal void UpdateEnqToSeen()
        {
            string query = "UPDATE enquiry SET seen = 1";
            SqlCommand cmd = new SqlCommand(query, Conn);
            Conn.Open();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }

        internal bool AddSubject(Subjects model)
        {
            bool result = false;
            try
            {
                string query = "INSERT INTO subjects (name) VALUES(@name)";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("name", model.Name));

                Conn.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                Conn.Close();
            }
            return result;
        }

        internal List<Subjects> AllSubjects()
        {
            DataTable td = new DataTable();
            List<Subjects> list = new List<Subjects>();
            try
            {
                string sqlquery = "SELECT * FROM subjects ORDER BY sub_id DESC";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                Conn.Open();
                adp.Fill(td);
                foreach (DataRow row in td.Rows)
                {
                    Subjects obj = new Subjects
                    {
                        ID = Convert.ToInt32(row["sub_id"]),
                        Name = Convert.ToString(row["name"]),
                    };

                    list.Add(obj);
                }
            }
            catch (Exception)
            { }
            finally
            {
                Conn.Close();
            }
            return list;
        }

        internal Subjects GetSubjectByID(int id)
        {
            DataTable td = new DataTable();
            Subjects obj = new Subjects();
            try
            {
                string sqlquery = "SELECT * FROM subjects where sub_id = @id";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("id", id));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                Conn.Open();

                adp.Fill(td);

                obj.ID = Convert.ToInt32(td.Rows[0]["sub_id"]);
                obj.Name = Convert.ToString(td.Rows[0]["name"]);
            }
            catch (Exception)
            { }
            finally
            {
                Conn.Close();
            }
            return obj;

        }

        internal bool UpdateSubject(Subjects model)
        {
            bool result = false;
            try
            {
                string query = "UPDATE subjects SET name = @name WHERE sub_id = @id";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.Add(new SqlParameter("name", model.Name));

                cmd.Parameters.Add(new SqlParameter("id", model.ID));

                Conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return result;
        }

        internal bool AddExam(Exam model)
        {
            bool result = false;
            try
            {
                string query = "INSERT INTO exams (title, description, rules, sub_id, passing_marks, datetime, status)" +
                    " VALUES(@title, @description, @rules, @sub_id, @passing_marks, @datetime, @status)";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("title", model.Title));
                cmd.Parameters.Add(new SqlParameter("description", model.Description));
                cmd.Parameters.Add(new SqlParameter("rules", model.Rules));
                cmd.Parameters.Add(new SqlParameter("sub_id", model.SubID));
                cmd.Parameters.Add(new SqlParameter("passing_marks", model.PassingMarks));
                cmd.Parameters.Add(new SqlParameter("datetime", model.DateTime));
                cmd.Parameters.Add(new SqlParameter("status", model.Status));

                Conn.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                Conn.Close();
            }
            return result;
        }

        internal List<Exam> AllExams()
        {
            DataTable td = new DataTable();
            List<Exam> list = new List<Exam>();
            try
            {
                string sqlquery = "SELECT * FROM exams ORDER BY exam_id DESC";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                Conn.Open();
                adp.Fill(td);
                Conn.Close();
                foreach (DataRow row in td.Rows)
                {
                    Exam obj = new Exam
                    {
                        ID = Convert.ToInt32(row["exam_id"]),
                        Title = Convert.ToString(row["title"]),
                        Description = Convert.ToString(row["description"]),
                        Rules = Convert.ToString(row["rules"]),
                        SubID = Convert.ToInt32(row["sub_id"]),
                        PassingMarks = Convert.ToInt32(row["passing_marks"]),
                        DateTime = Convert.ToDateTime(row["datetime"]),
                        Status = Convert.ToInt32(row["status"]),
                    };
                    obj.SubjectName = GetSubjectByID(obj.SubID).Name;
                    list.Add(obj);
                }
            }
            catch (Exception)
            { }
            return list;
        }

        internal bool UpdateExam(Exam model)
        {
            bool result = false;
            try
            {
                string query = "UPDATE exams SET title = @title, description = @description, rules = @rules, sub_id = @sub_id, passing_marks = @passing_marks, status = @status WHERE exam_id = @id";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.Add(new SqlParameter("title", model.Title));
                cmd.Parameters.Add(new SqlParameter("description", model.Description));
                cmd.Parameters.Add(new SqlParameter("rules", model.Rules));
                cmd.Parameters.Add(new SqlParameter("sub_id", model.SubID));
                cmd.Parameters.Add(new SqlParameter("passing_marks", model.PassingMarks));
                cmd.Parameters.Add(new SqlParameter("status", model.Status));

                cmd.Parameters.Add(new SqlParameter("id", model.ID));

                Conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
            return result;
        }

        internal Exam GetExamByID(int id)
        {
            DataTable td = new DataTable();
            Exam obj = new Exam();
            try
            {
                string sqlquery = "SELECT * FROM exams where exam_id = @id";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("id", id));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                Conn.Open();

                adp.Fill(td);

                Conn.Close();

                obj.ID = Convert.ToInt32(td.Rows[0]["exam_id"]);
                obj.Title = Convert.ToString(td.Rows[0]["title"]);
                obj.Description = Convert.ToString(td.Rows[0]["description"]);
                obj.Rules = Convert.ToString(td.Rows[0]["rules"]);
                obj.SubID = Convert.ToInt32(td.Rows[0]["sub_id"]);
                obj.PassingMarks = Convert.ToInt32(td.Rows[0]["passing_marks"]);
                obj.DateTime = Convert.ToDateTime(td.Rows[0]["datetime"]);
                obj.Status = Convert.ToInt32(td.Rows[0]["status"]);
                obj.SubjectName = GetSubjectByID(obj.SubID).Name;
            }
            catch (Exception)
            { }
            return obj;

        }
    }
}