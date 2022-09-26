using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Transactions;


namespace Project0
{
    public class Checking
    {
        public int checkingAccNumber { get; set; }
        public double accBalance { get; set; }
        public bool CheckingIsActive { get; set; }  
    

        public void TransferCheckingToSavings(int customerAccountNumber, int checkingAccountNumber, int savingsAccountNumber, decimal amount)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select accBalance from CheckingAccount where checkingAccNumber=@checkingAccountNumber", con);
            SqlCommand cmd2 = new SqlCommand("select accBalance from SavingsAccount where savingsAccNumber=@savingsAccountNumber", con);
            SqlCommand cmd3 = new SqlCommand("update CheckingAccount set accBalance=(accBalance - @amount) where checkingAccNumber=@checkingAccountNumber", con);
            SqlCommand cmd4 = new SqlCommand("update SavingsAccount set accBalance=(accBalance + @amount) where savingsAccNumber=@savingsAccountNumber", con);
            SqlCommand cmd5 = new SqlCommand("select checkingIsActive from CheckingAccount where checkingAccNumber=@checkingAccountNumber", con);
            SqlCommand cmd6 = new SqlCommand("select savingsIsActive from SavingsAccount where savingsAccNumber=@savingsAccountNumber", con);
            SqlCommand cmd7 = new SqlCommand("select checkingAccNumber from CheckingAccount where checkingAccNumber=@checkingAccountNumber", con);
            SqlCommand cmd8 = new SqlCommand("select savingsAccNumber from SavingsAccount where savingsAccNumber=@savingsAccountNumber", con);
            SqlCommand cmd9 = new SqlCommand("select accountNumber from Customer where accountNumber=@customerAccountNumber", con);
            SqlCommand cmd10 = new SqlCommand("select checkingAccNumber from CheckingAccount where checkingAccNumber=@checkingAccountNumber", con);
            SqlCommand cmd11 = new SqlCommand("select savingsAccNumber from SavingsAccount where savingsAccNumber=@savingsAccountNumber", con);

            cmd.Parameters.AddWithValue("@checkingAccountNumber", checkingAccountNumber);
            cmd2.Parameters.AddWithValue("@savingsAccountNumber", savingsAccountNumber );
            cmd3.Parameters.AddWithValue("@checkingAccountNumber", checkingAccountNumber);
            cmd3.Parameters.AddWithValue("@amount", amount);
            cmd4.Parameters.AddWithValue("@savingsAccountNumber", savingsAccountNumber );
            cmd4.Parameters.AddWithValue("@amount", amount);
            cmd5.Parameters.AddWithValue("@checkingAccountNumber", checkingAccountNumber);
            cmd6.Parameters.AddWithValue("@savingsAccountNumber", savingsAccountNumber);
            cmd7.Parameters.AddWithValue("@checkingAccountNumber", checkingAccountNumber);
            cmd8.Parameters.AddWithValue("@savingsAccountNumber", savingsAccountNumber);
            cmd9.Parameters.AddWithValue("@customerAccountNumber", customerAccountNumber);
            cmd10.Parameters.AddWithValue("@checkingAccountNumber", checkingAccountNumber);
            cmd11.Parameters.AddWithValue("@savingsAccountNumber", savingsAccountNumber);

            con.Open();
            decimal checkingBalance = Convert.ToDecimal(cmd.ExecuteScalar());
            decimal savingsBalance = Convert.ToDecimal(cmd2.ExecuteScalar());
            bool checkingIsActive = Convert.ToBoolean(cmd5.ExecuteScalar());
            bool savingsIsActive = Convert.ToBoolean(cmd6.ExecuteScalar());
            bool checkingAccValid = Convert.ToBoolean(cmd7.ExecuteScalar());
            bool savingsAccValid = Convert.ToBoolean(cmd8.ExecuteScalar());
            int validateCheckingAccount = Convert.ToInt32(cmd10.ExecuteScalar());
            int validateSavingsAccount = Convert.ToInt32(cmd11.ExecuteScalar());

            try
            {
                if (checkingAccValid && savingsAccValid && checkingIsActive && savingsIsActive && (checkingBalance >= amount) && (customerAccountNumber == validateCheckingAccount) && (customerAccountNumber == validateSavingsAccount))
                {
                    Console.Clear();                    
                    Console.WriteLine("Transfer successfully completed");
                    Console.WriteLine("You transferred: " + amount + " to your savings account" + "\n" + "Available funds in your checking account are now: " + (checkingBalance - amount) +
                                      "\n" + "Available funds in your savings account are now: " + (savingsBalance + amount));
                    SqlDataReader updateChecking = cmd3.ExecuteReader();
                    updateChecking.Close();
                    SqlDataReader updateSavings = cmd4.ExecuteReader();
                    updateSavings.Close();

                }
                else if (!checkingAccValid)
                {
                    throw new Exception("Invalid account number");
                }
                else if (!checkingIsActive)
                {
                    throw new Exception("This account is not active");
                }
                else if (amount >= checkingBalance)
                {
                    throw new Exception("Withdrawal was unsuccessful\n" + "Available funds are: " + checkingBalance + "\n" + "You attempted to withdraw: " + amount);
                }
                else if (customerAccountNumber != validateCheckingAccount)
                {
                    throw new Exception("You do not have access to transfer money from that checking account");
                }
                else if(customerAccountNumber != validateSavingsAccount)
                {
                    throw new Exception("You do not have access to transfer money to that savings account");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }          
            con.Close();

        }
        public void CreateCheckingAccount(int checkingAccNumber, double checkingAccountBalance)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("insert into CheckingAccount(checkingAccNumber, accBalance,checkingIsActive) values(@accNo,@balance,0)", con);
            SqlCommand cmd2 = new SqlCommand("update Customer set checkingAccNumber=@accNo where accountNumber=@checkingAccNumber", con);
            SqlCommand cmd3 = new SqlCommand("select count(*) from CheckingAccount where checkingAccNumber=@checkingAccNumber", con);

            cmd.Parameters.AddWithValue("@accNo", checkingAccNumber);
            cmd.Parameters.AddWithValue("@balance", checkingAccountBalance);
            cmd2.Parameters.AddWithValue("@accNo", checkingAccNumber);
            cmd2.Parameters.AddWithValue("@checkingAccNumber", checkingAccNumber);
            cmd3.Parameters.AddWithValue("@checkingAccNumber", checkingAccNumber);


            con.Open();
            int validateAccount = Convert.ToInt32(cmd3.ExecuteScalar());
            try {
                if (validateAccount == 0)
                {
                    cmd.ExecuteNonQuery();
                    cmd2.ExecuteReader();
                    con.Close();
                    Console.WriteLine(checkingAccNumber);
                    Console.WriteLine("Checking account created successfully");
                }
                else if(validateAccount != 0)
                {
                    throw new Exception("Checking account number is already in use");
                }
                }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public double depositToChecking(int chkAccNo, double depositAmount)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update CheckingAccount set accBalance= (accBalance + @depositBalance), checkingIsActive=2 where checkingAccNumber=@chkAccNo", con);
            SqlCommand cmd2 = new SqlCommand("select accBalance from CheckingAccount where checkingAccNumber=@chkAccNo", con);

            cmd.Parameters.AddWithValue("@depositBalance", depositAmount);
            cmd.Parameters.AddWithValue("@chkAccNo", chkAccNo);
            cmd2.Parameters.AddWithValue("@chkAccNo", chkAccNo);


            con.Open();
            cmd.ExecuteReader();
            con.Close();

            Console.WriteLine(accBalance );
            Console.WriteLine( depositAmount);
            Console.WriteLine("Deposit made successfully");
         return accBalance;
        }

        public void withdrawFromChecking(int chkAccNo, double withdrawalAmount)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update CheckingAccount set accBalance=(accBalance - @withdrawalAmount) where checkingAccNumber=@chkAccNo", con);
            SqlCommand cmd2 = new SqlCommand("select accBalance from CheckingAccount where checkingAccNumber=@chkAccNo", con);
            SqlCommand cmd3 = new SqlCommand("select checkingIsActive from CheckingAccount where checkingAccNumber=@chkAccNo", con);
            SqlCommand cmd4 = new SqlCommand("select checkingAccNumber from CheckingAccount where checkingAccNumber=@chkAccNo", con);


            cmd.Parameters.AddWithValue("@withdrawalAmount", withdrawalAmount);
            cmd.Parameters.AddWithValue("@chkAccNo", chkAccNo);
            cmd2.Parameters.AddWithValue("@chkAccNo", chkAccNo);
            cmd3.Parameters.AddWithValue("@chkAccNo", chkAccNo);
            cmd4.Parameters.AddWithValue("@chkAccNo", chkAccNo);

            con.Open();
            double balance = Convert.ToDouble(cmd2.ExecuteScalar());
            bool isActive = Convert.ToBoolean(cmd3.ExecuteScalar());
            bool accValid = Convert.ToBoolean(cmd4.ExecuteScalar());
            try
            {
                if (accValid && isActive && (balance >= withdrawalAmount))
                {
                    cmd.ExecuteReader();
                    Console.WriteLine("Withdrawal successfully completed");
                    Console.WriteLine("You withdrew: " + withdrawalAmount + "\n" + "Available funds are now: " + (balance - withdrawalAmount));
                }
                else if (!accValid)
                {
                    throw new Exception("Invalid account number");
                }
                else if (!isActive)
                {
                    throw new Exception("This checking account is not active");
                }
                else if (balance <= withdrawalAmount)
                {
                    throw new Exception("Withdrawal was unsuccessful\n" + "Available funds are: " + balance +"\n" + "You attempted to withdraw: " + withdrawalAmount);                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }          
            con.Close();                       
        }

            public void getCustomerID(int accountNumber){
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from Customer where accountNumber = @ID", con);

            cmd.Parameters.AddWithValue("@ID", accountNumber);
            con.Open();
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            Console.WriteLine(result);
            
    }
       /* public void UpdateCheckingAccount(double accBalance, int checkingAccNumber)
        {
            if(accBalance < 100)
            {
                throw new Exception("Please include an amount over $100");
            }
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from Customer where checkingAccNumber = @checkingAccNo", con);



            SqlCommand cmd2 = new SqlCommand("update CheckingAccount set accBalance=@accBalance", con);
            SqlCommand cmd3 = new SqlCommand("update CheckingAccount set transaction_1=@accBalance", con);
            Console.WriteLine(accBalance);
            Console.WriteLine(checkingAccNumber);
            cmd.Parameters.AddWithValue("@checkingAccNo", checkingAccNumber);
  
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
