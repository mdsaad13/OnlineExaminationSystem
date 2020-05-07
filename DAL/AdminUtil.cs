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

        /* Subject util starts here */
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
        /* Subject util ends here */

        /* Exam util starts here */
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

        internal bool UpdateExamStatus(int ExamID, int Status)
        {
            bool result = false;
            try
            {
                string query = "UPDATE exams SET status = @status WHERE exam_id = @exam_id";

                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("status", Status));

                cmd.Parameters.Add(new SqlParameter("exam_id", ExamID));

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
        /* Exam util ends here */

        /* Questions util starts here */
        internal bool AddExamQuestion(Question model)
        {
            bool result = false;
            try
            {
                string query = "INSERT INTO questions (exam_id, question, answer, opt1, opt2, opt3, opt4, marks)" +
                    " VALUES(@exam_id, @question, @answer, @opt1, @opt2, @opt3, @opt4, @marks)";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("exam_id", model.ExamID));
                cmd.Parameters.Add(new SqlParameter("question", model.Title));
                cmd.Parameters.Add(new SqlParameter("answer", model.Answer));
                cmd.Parameters.Add(new SqlParameter("opt1", model.Option1));
                cmd.Parameters.Add(new SqlParameter("opt2", model.Option2));
                cmd.Parameters.Add(new SqlParameter("opt3", model.Option3));
                cmd.Parameters.Add(new SqlParameter("opt4", model.Option4));
                cmd.Parameters.Add(new SqlParameter("marks", model.Marks));

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

        internal List<Question> AllExamQuestions(int ExamID, bool Random = false)
        {
            DataTable td = new DataTable();
            List<Question> list = new List<Question>();
            try
            {
                string Sort = "ORDER BY exam_id DESC";
                if (Random)
                {
                    Sort = "ORDER BY NEWID()";
                }

                string sqlquery = "SELECT * FROM questions WHERE exam_id = @exam_id "+ Sort;
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("exam_id", ExamID));
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                Conn.Open();
                adp.Fill(td);
                Conn.Close();
                foreach (DataRow row in td.Rows)
                {
                    Question obj = new Question
                    {
                        ID = Convert.ToInt32(row["ques_id"]),
                        ExamID = Convert.ToInt32(row["exam_id"]),
                        Title = Convert.ToString(row["question"]),
                        Answer = Convert.ToInt32(row["answer"]),
                        Option1 = Convert.ToString(row["opt1"]),
                        Option2 = Convert.ToString(row["opt2"]),
                        Option3 = Convert.ToString(row["opt3"]),
                        Option4 = Convert.ToString(row["opt4"]),
                        Marks = Convert.ToInt32(row["marks"]),
                    };
                    list.Add(obj);
                }
            }
            catch (Exception)
            { }
            return list;
        }

        internal Question GetQuestionByID(int id)
        {
            DataTable td = new DataTable();
            Question obj = new Question();
            try
            {
                string sqlquery = "SELECT * FROM questions where ques_id = @id";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("id", id));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                Conn.Open();

                adp.Fill(td);

                Conn.Close();

                obj.ID = Convert.ToInt32(td.Rows[0]["ques_id"]);
                obj.ExamID = Convert.ToInt32(td.Rows[0]["exam_id"]);
                obj.Title = Convert.ToString(td.Rows[0]["question"]);
                obj.Answer = Convert.ToInt32(td.Rows[0]["answer"]);
                obj.Option1 = Convert.ToString(td.Rows[0]["opt1"]);
                obj.Option2 = Convert.ToString(td.Rows[0]["opt2"]);
                obj.Option3 = Convert.ToString(td.Rows[0]["opt3"]);
                obj.Option4 = Convert.ToString(td.Rows[0]["opt4"]);
                obj.Marks = Convert.ToInt32(td.Rows[0]["marks"]);
            }
            catch (Exception)
            { }
            return obj;

        }

        internal bool UpdateExamQuestion(Question model)
        {
            bool result = false;
            try
            {
                string query = "UPDATE questions SET question = @question, answer = @answer, opt1 = @opt1, opt2 = @opt2, opt3 = @opt3, opt4 = @opt4, marks = @marks WHERE ques_id = @ques_id";

                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("question", model.Title));
                cmd.Parameters.Add(new SqlParameter("answer", model.Answer));
                cmd.Parameters.Add(new SqlParameter("opt1", model.Option1));
                cmd.Parameters.Add(new SqlParameter("opt2", model.Option2));
                cmd.Parameters.Add(new SqlParameter("opt3", model.Option3));
                cmd.Parameters.Add(new SqlParameter("opt4", model.Option4));
                cmd.Parameters.Add(new SqlParameter("marks", model.Marks));

                cmd.Parameters.Add(new SqlParameter("ques_id", model.ID));

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

        internal void DeleteQuestion(int ID)
        {
            try
            {
                string query = "DELETE from questions where ques_id = @id";
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.Add(new SqlParameter("@id", ID));
                Conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }

        /* Questions util ends here */
    }
}