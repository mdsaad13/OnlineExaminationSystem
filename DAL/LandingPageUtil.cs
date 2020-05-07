using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.DAL
{
    public class LandingPageUtil
    {
        private readonly SqlConnection Conn = new SqlConnection($"Data Source=localhost;Initial Catalog={AppInfo.DbName};Integrated Security=True");

        internal bool AddEnquiry(ContactUs model)
        {
            bool result = false;
            try
            {
                string query = "INSERT INTO enquiry (name, email, mobile, subject, message, datetime, seen) VALUES(@name, @email, @mobile, @subject, @message, @datetime, @seen)";
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.Add(new SqlParameter("name", model.Name));
                cmd.Parameters.Add(new SqlParameter("email", model.Email));
                cmd.Parameters.Add(new SqlParameter("mobile", model.Mobile));
                cmd.Parameters.Add(new SqlParameter("subject", model.Sub));
                cmd.Parameters.Add(new SqlParameter("message", model.Message));
                cmd.Parameters.Add(new SqlParameter("datetime", model.DateTime));
                cmd.Parameters.Add(new SqlParameter("seen", model.Seen));

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
    }
}