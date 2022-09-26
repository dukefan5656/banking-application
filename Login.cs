using System;
using System.Data.SqlClient;
using System.Reflection;
using Project0;

namespace Project0
{
    public class Login {
        public bool CheckLoginAdmin(string uName, string pwd, bool isAdmin = true)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select count(*) from Customer where userName = @uName and accPassword = @pwd and isAdmin=1", con);
            
            cmd.Parameters.AddWithValue("@uName", uName);
            cmd.Parameters.AddWithValue("@pwd", pwd);
            
            con.Open();
            int result = (int)cmd.ExecuteScalar();
            try
            {
                if (result == 1)
                {
                    return true;
                }
                else if(result != 1)
                {
                    throw new Exception("Username or password is incorrect\nPress Enter to return to the main menu");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            return false;
            con.Close();
        }
        public bool CheckLoginCustomer(string uName, string pwd, bool isAdmin = true)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select count(*) from Customer where userName = @uName and accPassword = @pwd", con);

            cmd.Parameters.AddWithValue("@uName", uName);
            cmd.Parameters.AddWithValue("@pwd", pwd);

            con.Open();
            int result = (int)cmd.ExecuteScalar();
            try
            {
                if (result == 1)
                {
                    return true;
                }
                else if (result != 1)
                {
                    throw new Exception("Username or password is incorrect\nPress Enter to return to the main menu");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return false;
        }
    }
}
