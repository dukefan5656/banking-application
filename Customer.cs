using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Transactions;
using System.Reflection.Metadata.Ecma335;
using Project0;
using System.Linq.Expressions;

namespace Project0
{


    public class Customer
    {
        public int accountNumber { get; set; }
        public int checkingAccNumber { get; set; }
        public int savingsAccNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string accPassword { get; set; }
        public string email { get; set; }
        public bool isAdmin { get; set; }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
            public string HidePassword()
            {
                ConsoleKeyInfo key;
                string code = "";
                do
                {
                    key = Console.ReadKey(true);

                    if (Char.IsNumber(key.KeyChar) || Char.IsLetter(key.KeyChar))
                    {
                        Console.Write("*");
                        code += key.KeyChar.ToString();
                    }

                } while (key.Key != ConsoleKey.Enter);

                int pwdLength = code.Length / 2;
                code.Remove(0, pwdLength).TrimEnd();
                return code;
            }
        
        public void AddNewCustomer(int accNo)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            Console.WriteLine("Please enter the customer's first name");
            string first = Console.ReadLine();
            Console.WriteLine("Please enter the customer's last name");
            string last = Console.ReadLine();
            Console.WriteLine("Please enter the customer's username");
            string username = Console.ReadLine();
            Console.WriteLine("Please enter the customer's password");
            string password = HidePassword();
            Console.WriteLine("\nPlease enter the customer's email");
            string email = Console.ReadLine();
            bool isValid = IsValidEmail(email);
            Console.WriteLine("Is the user an admin? true or false");
            bool admin = Convert.ToBoolean(Console.ReadLine());
            int chkAcc = accNo;
            int savAcc = accNo;
            SqlCommand cmd2 = new SqlCommand("select accountNumber from Customer where accountNumber=@accNo", con);
            SqlCommand cmd = new SqlCommand("insert into Customer values(@accNo,@chkNo,@savNo,@first,@last,@username,@password,@email,@admin)", con);
            cmd2.Parameters.AddWithValue("accNo", accNo);
            cmd.Parameters.AddWithValue("@accNo", accNo);
            cmd.Parameters.AddWithValue("@chkNo", chkAcc);
            cmd.Parameters.AddWithValue("@savNo", savAcc);
            cmd.Parameters.AddWithValue("@first", first);
            cmd.Parameters.AddWithValue("@last", last);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@admin", admin);

            con.Open();
            try
            {

                int accountNumber = Convert.ToInt32(cmd2.ExecuteScalar());
                if (accountNumber != accNo && isValid)
                {
                    SqlDataReader addAccount = cmd.ExecuteReader();
                    addAccount.Close();
                    Console.WriteLine("Congratulations, account was created successfully!");
                    
                }
                else if (accNo.GetType() != typeof(int))
                {
                    throw new Exception("Account number must consist of only numbers\nPress ENTER to return to the main menuu");
                }
                else if(accountNumber == accNo)
                {
                    throw new Exception("Customer account number is already in use");
                }
                else if (!isValid)
                {
                    throw new Exception("Email format is not valid");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            GetCustomerByID(accNo, username, password);
        }
        public string DeleteCustomerAccount(int accountNumber, bool deleteAccount)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("Delete from Customer where accountNumber=@accountNumber", con);
            SqlCommand cmd2 = new SqlCommand("Delete from SavingsAccount where savingsAccNumber=@accountNumber", con);
            SqlCommand cmd3 = new SqlCommand("Delete from CheckingAccount where checkingAccNumber=@accountNumber", con);
            
            cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
            cmd2.Parameters.AddWithValue("@accountNumber", accountNumber);
            cmd3.Parameters.AddWithValue("@accountNumber", accountNumber);
            if (deleteAccount)
            {
                con.Open();
                SqlDataReader deleteCustomerAccount = cmd.ExecuteReader();
                deleteCustomerAccount.Close();
                SqlDataReader deleteSavingsAccount = cmd2.ExecuteReader();
                deleteSavingsAccount.Close();
                SqlDataReader deleteCheckingAccount = cmd3.ExecuteReader();
                deleteCheckingAccount.Close();
                con.Close();
            }
            else
            {
                Console.WriteLine("Account was not deleted");
            }
            Console.WriteLine("Account was successfully deleted");
            return "okay";
        }
        public void UpdateCustomerUsername(int userID, string username)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update Customer set userName=@username where accountNumber = @userID", con);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@username", username);
            con.Open();
            cmd.ExecuteScalar();
            Console.WriteLine(username);
            con.Close();
        }
        public void UpdateCustomerPassword(int userID, string password)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update Customer set accPassword=@password where accountNumber = @userID", con);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@password", password);
            con.Open();
            string newPassword = Convert.ToString(cmd.ExecuteScalar());          
            Console.WriteLine("new password is: " + newPassword);
            con.Close();
        }

        public string UpdateCustomerEmail(int userID, string email)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update Customer set email=@email where accountNumber = @userID", con);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@userID", userID);

            con.Open();
            cmd.ExecuteScalar();           
            string newEmail = Convert.ToString(cmd.ExecuteScalar());           
            con.Close();
            return newEmail;
        }

        public bool CheckIsAdmin(int userID)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select isAdmin from Customer where accountNumber=@userID", con);
            cmd.Parameters.AddWithValue("@userID", userID);

            con.Open();
            bool isAdmin = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();
            return isAdmin;
        }

        public bool ChangeAdmin(int userID, bool admin)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("update Customer set isAdmin=@admin where accountNumber = @userID", con);
            cmd.Parameters.AddWithValue("@admin", admin);
            cmd.Parameters.AddWithValue("@userID", userID);

            con.Open();
            bool isAdmin = Convert.ToBoolean(cmd.ExecuteScalar());
            Console.WriteLine("Admin has been changed to: " + isAdmin);
            con.Close();
            return isAdmin;
        }

        public void UpdateCustomerRecord(int customerID)
        {           
            CheckIsAdmin(customerID);
            if (CheckIsAdmin(customerID))
            {
                Console.WriteLine("Please enter the account number associated with the customer you would like to edit");
                customerID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Which record would you like to update?");
                Console.WriteLine("1. Username");
                Console.WriteLine("2. Password");
                Console.WriteLine("3. Email");
                Console.WriteLine("4. Adjust admin access for user");
                Console.WriteLine("5. Exit");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:

                        Console.WriteLine("Please enter new username");
                        string username = Console.ReadLine();

                        UpdateCustomerUsername(customerID, username);
                        break;
                    case 2:
                        Console.WriteLine("Please enter your new password");
                        string password = Console.ReadLine();
                        UpdateCustomerPassword(customerID, password);
                        break;
                    case 3:
                        Console.WriteLine("Please enter the user's new email address");
                        string email = Console.ReadLine();
                        UpdateCustomerEmail(customerID, email);
                        Console.WriteLine("Customers new email has been created successfully");
                        break;

                    case 4:
                        Console.WriteLine("Adjust admin privileges");
                        Console.WriteLine("Enter true to make user an admin");
                        Console.WriteLine("Enter false to remove admin access");
                        bool adminAccess = Convert.ToBoolean(Console.ReadLine());
                        ChangeAdmin(customerID, adminAccess);
                        Console.WriteLine(adminAccess);
                        break;
                    case 5:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Please enter the account number associated with the customer you would like to edit");
                Console.WriteLine("Which record would you like to update?");
                Console.WriteLine("1. Username");
                Console.WriteLine("2. Password");
                Console.WriteLine("3. Email");
                Console.WriteLine("4. Exit");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please enter your new username");
                        string username = Console.ReadLine();
                        UpdateCustomerUsername(customerID, username);
                        break;
                    case 2:
                        Console.WriteLine("Please enter your new password");
                        string password = Console.ReadLine();
                        UpdateCustomerPassword(customerID, password);
                        break;
                    case 3:
                        Console.WriteLine("Please enter the user's new email address");
                        string email = Console.ReadLine();
                        UpdateCustomerEmail(customerID, email);
                        break;
                }
            }
        }

        public int GetCustomerID(string username, string password)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select accountNumber from Customer where userName=@username and accPassword=@password", con);

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            con.Open();
            int ID = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            Console.WriteLine(ID);
            return ID;
        }

        public Customer GetCustomerByID(int p_accNo, string p_uName, string p_uPassword)
        {
            SqlConnection con = new SqlConnection("server=HOMEPC\\CHRISSERVER; database=Bank;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("select * from Customer where accountNumber = @accNo", con);
            cmd.Parameters.AddWithValue("@accNo", p_accNo);
            con.Open();

            SqlDataReader readRow = cmd.ExecuteReader();
            if (readRow.Read())

            {
                Customer customer = new Customer()
                {
                    accountNumber = Convert.ToInt32(readRow[0]),
                    firstName = readRow[3].ToString(),
                    lastName = readRow[4].ToString(),
                    userName = readRow[5].ToString(),
                    accPassword = readRow[6].ToString(),
                    email = readRow[7].ToString(),
                    isAdmin = Convert.ToBoolean(readRow[8])
                };                
                if (p_uName == customer.userName && p_uPassword == customer.accPassword){
                    Console.WriteLine(customer.firstName);
                    Console.WriteLine(customer.lastName);
                    Console.WriteLine(customer.userName);
                    //Console.WriteLine(customer.accPassword);
                    Console.WriteLine(customer.email);
                    Console.WriteLine("Admin: " + customer.isAdmin);
                    return customer;
                    
                }
                Console.WriteLine("You are not authorized to view this account");
            }
            else
            {
                Console.WriteLine("Account not found");
            }
            return null;
        }
    } 
}
