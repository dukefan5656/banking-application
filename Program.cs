using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Project0;
using System.Data;
using System.Collections.Concurrent;

Login loginObj = new Login();
Hider hiderObj = new Hider();
Checking chkObj = new Checking();
Customer customerObj = new Customer();
Savings savObj = new Savings();

bool activeSession = true;
/*select count(*) from Customer where userName = @uName and accPassword = @pwd
*/
while (activeSession)
{
    Console.Clear();
    Console.WriteLine(" -----------------------------");
    Console.WriteLine("|                             |");
    Console.WriteLine("| WELCOME TO FINANCIAL RUIN!! |");
    Console.WriteLine("|                             |");
    Console.WriteLine(" -----------------------------\n");
    Console.WriteLine("Please select the user type to ruin!... We mean, log into\n");
    Console.WriteLine("1. Admin");
    Console.WriteLine("2. Customer");

    int choice = Convert.ToInt32(Console.ReadLine());
    if (choice > 3)
    {
        activeSession = false;
    }
    switch (choice)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("Please enter your username");
            string uName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Please enter your password");

            string pwd = hiderObj.HidePassword();
            int globalAccNo = customerObj.GetCustomerID(uName, pwd);
            if (loginObj.CheckLoginAdmin(uName, pwd))
            {
                Console.Clear();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. Create a new customer account");
                Console.WriteLine("2. View customer's account details");
                Console.WriteLine("3. Deposit to an existing checking account");
                Console.WriteLine("4. Withdraw from an existing checking account");
                Console.WriteLine("5. Deposit to an existing savings account");
                Console.WriteLine("6. Withdraw from an existing savings account");
                Console.WriteLine("7. Update an existing customer's record");
                Console.WriteLine("8. Delete a customer's account");
                Console.WriteLine("9. Exit to the main menu");

                choice = Convert.ToInt32(Console.ReadLine());
                if (choice > 10)
                {
                    Console.WriteLine("Invalid selection");
                    return;
                }
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please enter a new and unique account number");
                        int accNo = Convert.ToInt32(Console.ReadLine());
                        chkObj.CreateCheckingAccount(accNo, 0);
                        savObj.CreateSavingsAccount(accNo, 0);
                        Console.Clear();
                        customerObj.AddNewCustomer(accNo);
                        break;

                    case 2:
                        Console.WriteLine("Enter the account number that you would like to look up");
                        int customerID = Convert.ToInt32(Console.ReadLine());
                        Customer customerDetails = customerObj.GetCustomerByID(customerID, uName, pwd);
                        Console.WriteLine("Customer's Account number: " + customerDetails.accountNumber);
                        Console.WriteLine("Customer's first name: " + customerDetails.firstName);
                        Console.WriteLine("Customer's last name: " + customerDetails.lastName);
                        Console.WriteLine("Customer's username: " + customerDetails.userName);
                        Console.WriteLine("Customer's password: " + customerDetails.accPassword);
                        Console.WriteLine("Customer's email: " + customerDetails.email);
                        Console.WriteLine("Is the customer an admin?: " + customerDetails.isAdmin);
                        break;

                    case 3:
                        Console.WriteLine("Enter the account number you would like to deposit into");
                        int ID = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please choose an amount to deposit (must be at least $100)");
                        int balance = Convert.ToInt32(Console.ReadLine());
                        chkObj.depositToChecking(ID, balance);
                        break;

                    case 4:
                        Console.WriteLine("Enter the account number you would like to withdraw from");
                        ID = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please choose an amount to withdraw (must be at least $100)");
                        balance = Convert.ToInt32(Console.ReadLine());
                        chkObj.withdrawFromChecking(ID, balance);
                        break;

                    case 5:
                        Console.WriteLine("Enter the account number you would like to deposit to");
                        ID = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please choose an amount to deposit (must be at least $100)");
                        balance = Convert.ToInt32(Console.ReadLine());
                        savObj.depositToSavings(ID, balance);
                        break;

                    case 6:
                        Console.WriteLine("Enter the account number you would like to withdraw from");
                        ID = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please choose an amount to withdraw (must be at least $100)");
                        balance = Convert.ToInt32(Console.ReadLine());
                        savObj.withdrawFromSavings(ID, balance);
                        break;

                    case 7:
                        Console.Clear();
                        customerObj.UpdateCustomerRecord(globalAccNo);
                        break;

                    case 8:
                        Console.Clear();
                        Console.WriteLine("Are you sure you want to delete this user's account?\nType true to delete or false to exit");
                        bool deleteAccount = Convert.ToBoolean(Console.ReadLine());
                        customerObj.DeleteCustomerAccount(globalAccNo, deleteAccount);
                        break;

                    case 9:                        
                        break;
                }
            }
            break;
        case 2:
            Console.Clear();
            Console.WriteLine("Please enter your username");
            uName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Please enter your password");

            pwd = hiderObj.HidePassword();

            loginObj.CheckLoginCustomer(uName, pwd);
            globalAccNo = customerObj.GetCustomerID(uName, pwd);
            if (loginObj.CheckLoginCustomer(uName, pwd))
            {
                Console.Clear();
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1. Check account details");
                Console.WriteLine("2. Deposit to checking account");
                Console.WriteLine("3. Withdraw from checking account");
                Console.WriteLine("4. Transfer to your savings account");
                Console.WriteLine("5. Update your account details");
                Console.WriteLine("6. Delete your account");
                Console.WriteLine("7. Return to the main menu");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Please enter your account number");
                        int customerID = Convert.ToInt32(Console.ReadLine());
                        customerObj.GetCustomerByID(customerID, uName, pwd);
                        Customer customerDetails = customerObj.GetCustomerByID(customerID, uName, pwd);
                        Console.WriteLine("Customer's Account number: " + customerDetails.accountNumber);
                        Console.WriteLine("Customer's first name: " + customerDetails.firstName);
                        Console.WriteLine("Customer's last name: " + customerDetails.lastName);
                        Console.WriteLine("Customer's username: " + customerDetails.userName);
                        Console.WriteLine("Customer's password: " + customerDetails.accPassword);
                        Console.WriteLine("Customer's email: " + customerDetails.email);
                        Console.WriteLine("Is the customer an admin?: " + customerDetails.isAdmin);
                        break;
                    case 2:
                        Console.Clear();
                        customerID = globalAccNo;

                        Console.WriteLine("Please enter the amount to deposit");
                        double depositAmount = Convert.ToDouble(Console.ReadLine());
                        chkObj.depositToChecking(customerID, depositAmount);
                        break;
                    case 3:
                        Console.Clear();
                        customerID = globalAccNo;

                        Console.WriteLine("Please enter the amount to withdraw");
                        depositAmount = Convert.ToDouble(Console.ReadLine());
                        chkObj.withdrawFromChecking(customerID, depositAmount);
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Please enter the checking account number");
                        int checkingAccNumber = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please enter the savings account number");
                        int savingsAccNumber = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please enter the amount that you wish to transfer");
                        decimal amount = Convert.ToDecimal(Console.ReadLine());
                        chkObj.TransferCheckingToSavings(globalAccNo, checkingAccNumber, savingsAccNumber, amount);
                        break;
                    case 5:
                        Console.Clear();
                        customerObj.UpdateCustomerRecord(globalAccNo);                        
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("Are you sure you want to delete your account?\nType true to delete or false to exit");
                        bool deleteAccount = Convert.ToBoolean(Console.ReadLine());
                        customerObj.DeleteCustomerAccount(globalAccNo, deleteAccount);
                        break;
                    case 7:
                        break;
                }              
            }
            break;
        default:
            Console.WriteLine("invalid selection, please press ENTER to return to the main menu");
            activeSession = true;
            break;
    }
    Console.ReadLine();
}



