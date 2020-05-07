using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.DAL
{
    public class AccountUtil
    {
        private readonly SqlConnection Conn = new SqlConnection($"Data Source=localhost;Initial Catalog={AppInfo.DbName};Integrated Security=True");
        
        public int UserLogin(string Email, string Pass)
        {
            // default Login failed
            int result = 0;
            try
            {
                string query = "SELECT * FROM users WHERE email = @email AND password = @password";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("email", Email));
                cmd.Parameters.Add(new SqlParameter("password", Pass));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int Status = Convert.ToInt32(dt.Rows[0]["status"]);
                    if (Status == 0)
                    {
                        // Account Blocked
                        result = 1;
                    }
                    else
                    {
                        // Login success
                        result = 2;
                        HttpContext.Current.Session["StudentID"] = dt.Rows[0]["user_id"];
                        HttpContext.Current.Session["Name"] = dt.Rows[0]["name"];
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public bool AdminLogin(string Email, string Pass)
        {
            bool result = false;
            try
            {
                string query = "SELECT * FROM admin WHERE email = @email AND password = @password";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("email", Email));
                cmd.Parameters.Add(new SqlParameter("password", Pass));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                    HttpContext.Current.Session["AdminID"] = dt.Rows[0]["admin_id"];
                    HttpContext.Current.Session["Name"] = dt.Rows[0]["name"];
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        internal bool AddUser(User model)
        {
            bool result = false;
            try
            {
                string query = "INSERT INTO users (name, email, mobile, password, status, created_date) VALUES(@name, @email, @mobile, @password, @status, @created_date)";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("name", model.Name));
                cmd.Parameters.Add(new SqlParameter("email", model.Email));
                cmd.Parameters.Add(new SqlParameter("mobile", model.Mobile));
                cmd.Parameters.Add(new SqlParameter("password", model.Password));
                cmd.Parameters.Add(new SqlParameter("status", model.Status));
                cmd.Parameters.Add(new SqlParameter("created_date", model.DateTime));

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

        internal List<User> AllUsers()
        {
            DataTable td = new DataTable();
            List<User> list = new List<User>();
            try
            {
                string sqlquery = "SELECT * FROM users ORDER BY user_id DESC";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                Conn.Open();
                adp.Fill(td);
                foreach (DataRow row in td.Rows)
                {
                    User obj = new User
                    {
                        ID = Convert.ToInt32(row["user_id"]),
                        Name = Convert.ToString(row["name"]),
                        Email = Convert.ToString(row["email"]),
                        Mobile = Convert.ToString(row["mobile"]),
                        Password = Convert.ToString(row["password"]),
                        Status = Convert.ToInt32(row["status"]),
                        DateTime = Convert.ToDateTime(row["created_date"])
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

        internal User GetUserByID(int id)
        {
            DataTable td = new DataTable();
            User obj = new User();
            try
            {
                string sqlquery = "SELECT * FROM users where user_id = @id";
                SqlCommand cmd = new SqlCommand(sqlquery, Conn);
                cmd.Parameters.Add(new SqlParameter("id", id));

                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                Conn.Open();

                adp.Fill(td);

                obj.ID = Convert.ToInt32(td.Rows[0]["user_id"]);
                obj.Name = Convert.ToString(td.Rows[0]["name"]);
                obj.Email = Convert.ToString(td.Rows[0]["email"]);
                obj.Mobile = Convert.ToString(td.Rows[0]["mobile"]);
                obj.Password = Convert.ToString(td.Rows[0]["password"]);
                obj.Status = Convert.ToInt32(td.Rows[0]["status"]);
                obj.DateTime = Convert.ToDateTime(td.Rows[0]["created_date"]);
            }
            catch (Exception)
            { }
            finally
            {
                Conn.Close();
            }
            return obj;

        }

        internal bool UpdateUser(User model)
        {
            bool result = false;
            try
            {
                string query = "UPDATE users SET name = @name, email = @email, mobile = @mobile, status = @status WHERE user_id = @id";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.Add(new SqlParameter("name", model.Name));
                cmd.Parameters.Add(new SqlParameter("email", model.Email));
                cmd.Parameters.Add(new SqlParameter("mobile", model.Mobile));
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

    }
}