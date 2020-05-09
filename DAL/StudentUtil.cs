using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace OnlineExaminationSystem.DAL
{
    public class StudentUtil
    {
        private readonly SqlConnection Conn = new SqlConnection($"Data Source=localhost;Initial Catalog={AppInfo.DbName};Integrated Security=True");

        internal bool NewExam(Result model)
        {
            bool result = false;
            try
            {
                string query = "INSERT INTO results (result_id, user_id, exam_id, attended_date) VALUES(@result_id, @user_id, @exam_id, @attended_date)";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("result_id", model.ID));
                cmd.Parameters.Add(new SqlParameter("user_id", model.UserID));
                cmd.Parameters.Add(new SqlParameter("exam_id", model.ExamID));
                cmd.Parameters.Add(new SqlParameter("attended_date", model.AttendDate));

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

        internal void InsertAnswer(int ResultID, int QuestionID, int Answer)
        {
            try
            {
                string query = "INSERT INTO results_details (result_id, ques_id, answer) VALUES(@result_id, @ques_id, @answer)";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("result_id", ResultID));
                cmd.Parameters.Add(new SqlParameter("ques_id", QuestionID));
                cmd.Parameters.Add(new SqlParameter("answer", Answer));

                Conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {

            }
            finally
            {
                Conn.Close();
            }
        }

        internal List<Result> AllResults(int UserID)
        {
            DataTable td = new DataTable();
            AdminUtil adminUtil = new AdminUtil();
            Exam exam = new Exam();
            List<Result> list = new List<Result>();

            try
            {
                string sqlquery = "SELECT * FROM results WHERE user_id = @user_id ORDER BY attended_date DESC";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("user_id", UserID));
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                Conn.Open();
                adp.Fill(td);
                Conn.Close();

                foreach (DataRow row in td.Rows)
                {
                    Result obj = new Result
                    {
                        ID = Convert.ToInt32(row["result_id"]),
                        UserID = Convert.ToInt32(row["user_id"]),
                        ExamID = Convert.ToInt32(row["exam_id"]),
                        AttendDate = Convert.ToDateTime(row["attended_date"]),
                    };

                    exam = adminUtil.GetExamByID(obj.ExamID);
                    obj.ExamTitle = exam.Title;
                    obj.Subject = exam.SubjectName;
                    obj.Status = ResultStatus(obj.ID, obj.ExamID);

                    list.Add(obj);
                }
            }
            catch (Exception)
            { }
            return list;
        }

        internal Result GetResultByID(int id)
        {
            DataTable td = new DataTable();
            Result obj = new Result();
            try
            {
                string sqlquery = "SELECT * FROM results where result_id = @id";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("id", id));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                Conn.Open();
                adp.Fill(td);
                Conn.Close();

                obj.ID = Convert.ToInt32(td.Rows[0]["result_id"]);
                obj.UserID = Convert.ToInt32(td.Rows[0]["user_id"]);
                obj.ExamID = Convert.ToInt32(td.Rows[0]["exam_id"]);
                obj.AttendDate = Convert.ToDateTime(td.Rows[0]["attended_date"]);
                obj.Status = ResultStatus(obj.ID, obj.ExamID);
            }
            catch (Exception)
            { }
            finally
            {
                
            }
            return obj;

        }

        private ResultStatus ResultStatus(int ResultID, int ExamID)
        {
            DataTable td = new DataTable();
            ResultStatus obj = new ResultStatus();
            AdminUtil adminUtil = new AdminUtil();
            General general = new General();
            try
            {
                string sqlquery = "SELECT * FROM results_details where result_id = @result_id";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("result_id", ResultID));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                Conn.Open();
                adp.Fill(td);
                Conn.Close();

                int AnswerGiven;
                obj.MarksScored = 0;

                Question question = new Question();

                int PassingMarks = adminUtil.GetExamByID(ExamID).PassingMarks;
                obj.TotalMarks = Convert.ToInt32(general.SumByArgs("questions", "marks", "exam_id = "+ ExamID));

                foreach (DataRow row in td.Rows)
                {
                    question = adminUtil.GetQuestionByID(Convert.ToInt32(row["ques_id"]));
                    AnswerGiven = Convert.ToInt32(row["answer"]);
                    if (question.Answer == AnswerGiven)
                    {
                        obj.MarksScored+= question.Marks;
                    }
                }
                if (obj.MarksScored >= PassingMarks)
                {
                    obj.StatusInInt = 1;
                    obj.StatusInStr = "Pass";
                }
                else
                {
                    obj.StatusInInt = 0;
                    obj.StatusInStr = "Failed";
                }

                double TempMarksScored = Convert.ToDouble(obj.MarksScored);
                double TempTotalMarks = Convert.ToDouble(obj.TotalMarks);

                obj.Percentage = (TempMarksScored / TempTotalMarks) * 100;
            }
            catch (Exception ex)
            { }
            return obj;
        }

        internal List<Question> ExamQuestionsWithResult(int ExamID, int ResultID)
        {
            DataTable td = new DataTable();
            List<Question> list = new List<Question>();
            try
            {
                string sqlquery = "SELECT * FROM questions WHERE exam_id = @exam_id ORDER BY exam_id DESC";
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
                    obj.UserAnswered = GetUserAnsweredForQuestion(ResultID, obj.ID);
                    list.Add(obj);
                }
            }
            catch (Exception)
            { }
            return list;
        }

        private int GetUserAnsweredForQuestion(int ResultID, int QuesID)
        {
            DataTable td = new DataTable();
            int Ans = 0;
            try
            {
                string sqlquery = "SELECT * FROM results_details where result_id = @result_id AND ques_id = @ques_id";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("result_id", ResultID));
                cmd.Parameters.Add(new SqlParameter("ques_id", QuesID));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                Conn.Open();
                adp.Fill(td);
                Conn.Close();

                Ans = Convert.ToInt32(td.Rows[0]["answer"]);
            }
            catch (Exception)
            { }
            return Ans;
        }
    }
}