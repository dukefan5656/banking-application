using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace Project0
{
    public class Savings
    {
        public int savingsAccNumber { get; set; }
        public double savingsAccBalance { get; set; }
        public bool SavingsIsActive { get; set; }

        public void CreateSavingsAccount(int savingsAccNumber, double savingsAccountBalance)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("insert into SavingsAccount(savingsAccNumber, accBalance,savingsIsActive) values(@accNo,0,0)", con);
            SqlCommand cmd2 = new SqlCommand("update Customer set savingsAccNumber=@accNo where savingsAccNumber=@savingsAccNumber", con);
            SqlCommand cmd3 = new SqlCommand("select count(*) from SavingsAccount where savingsAccNumber=@savingsAccNumber", con);

            cmd.Parameters.AddWithValue("@accNo", savingsAccNumber);
            cmd.Parameters.AddWithValue("@balance", savingsAccountBalance);
            cmd2.Parameters.AddWithValue("@accNo", savingsAccNumber);
            cmd2.Parameters.AddWithValue("@savingsAccNumber", savingsAccNumber);
            cmd3.Parameters.AddWithValue("@savingsAccNumber", savingsAccNumber);
            con.Open();
            int validateAccount = Convert.ToInt32(cmd3.ExecuteScalar());
            try
            {
                if (validateAccount == 0)
                {
                    cmd.ExecuteNonQuery();
                    cmd2.ExecuteReader();
                    con.Close();
                    Console.WriteLine(savingsAccNumber);
                    Console.WriteLine("Savings account created successfully");
                }
                else if (validateAccount != 0)
                {
                    throw new Exception("Savings account number is already in use");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void depositToSavings(int savAccNo, double depositAmount)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update SavingsAccount set accBalance= (accBalance + @depositAmount) where savingsAccNumber=@savAccNo", con);
            SqlCommand cmd2 = new SqlCommand("select accBalance from SavingsAccount where savingsAccNumber=@savAccNo", con);
            SqlCommand cmd3 = new SqlCommand("select count(*) from SavingsAccount where savingsAccNumber=@savAccNo", con);

            cmd.Parameters.AddWithValue("@depositAmount", depositAmount);
            cmd.Parameters.AddWithValue("@savAccNo", savAccNo);
            cmd2.Parameters.AddWithValue("@savAccNo", savAccNo);
            cmd3.Parameters.AddWithValue("@savAccNo", savAccNo);

            con.Open();
            int validateAccount = Convert.ToInt32(cmd3.ExecuteScalar());
            try
            {
                if (validateAccount == 1)
                {
                    double savingsInitialBalance = Convert.ToDouble(cmd2.ExecuteScalar());
                    cmd.ExecuteReader();
                    Console.WriteLine("Deposit made successfully\nDeposited amount: " + depositAmount + "\nRemaining balance in savings account: " + (savingsInitialBalance + depositAmount));
                }
                else if(validateAccount == 0)
                {
                    throw new Exception("Account does not exist");
                }
            }    
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
        }

        public void withdrawFromSavings(int savAccNo, double withdrawAmount)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update SavingsAccount set accBalance=(accBalance - @withdrawAmount) where savingsAccNumber=@savAccNo", con);
            SqlCommand cmd2 = new SqlCommand("select accBalance from SavingsAccount where savingsAccNumber=@savAccNo", con);
            SqlCommand cmd3 = new SqlCommand("select count(*) from SavingsAccount where savingsAccNumber=@savAccNo", con);

            cmd.Parameters.AddWithValue("@withdrawAmount", withdrawAmount);
            cmd.Parameters.AddWithValue("@savAccNo", savAccNo);
            cmd2.Parameters.AddWithValue("@savAccNo", savAccNo);
            cmd3.Parameters.AddWithValue("@savAccNo", savAccNo);

            con.Open();
            int validateAccount = Convert.ToInt32(cmd3.ExecuteScalar());
            try
            {
                if (validateAccount == 1)
                {
                    double savingsInitialBalance = Convert.ToDouble(cmd2.ExecuteScalar());
                    cmd.ExecuteReader();
                    Console.WriteLine("Withdrawl made successfully\nWithdrawn amount: " + withdrawAmount + "\nRemaining balance in savings account: " + (savingsInitialBalance - withdrawAmount));
                }
                else if (validateAccount == 0)
                {
                    throw new Exception("Account does not exist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
        }
        public void getCustomerID(int accountNumber)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from Customer where accountNumber = @ID", con);

            cmd.Parameters.AddWithValue("@ID", accountNumber);
            con.Open();
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
        }
       /* public void UpdateSavingsAccount(double accBalance, int savingsAccNumber)
        {
            if (accBalance < 100)
            {
                throw new Exception("Please include an amount over $100");
            }
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from Customer where savingsAccNumber = @savingsAccNo", con);



            SqlCommand cmd2 = new SqlCommand("update SavingsAccount set accBalance=@accBalance", con);
            SqlCommand cmd3 = new SqlCommand("update SavingsAccount set transaction_1=@accBalance", con);
            Console.WriteLine(accBalance);
            Console.WriteLine(savingsAccNumber);
            cmd.Parameters.AddWithValue("@savingsAccNo", savingsAccNumber);

            cmd2.Parameters.AddWithValue("@accBalance", accBalance);
            cmd3.Parameters.AddWithValue("accBalance", accBalance);
            con.Open();
            cmd.ExecuteReader();
            con.Close();


            con.Open();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery().ToString();
            con.Close();

        }*/
    }
}
