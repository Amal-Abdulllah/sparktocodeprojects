namespace BankSystemMINIproject
{
    internal class Program
    {
        static List<string> customerNames = new List<string>();
        static List<string> accountNumbers = new List<string>();
        static List<double> balances = new List<double>();

        static void Main(string[] args)
        {
            bool exitApp = false;

            while (!exitApp)
            {
                Console.WriteLine("\n===== Welcome to Spark Bank =====");
                Console.WriteLine("1. Add New Account");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Show Balance");
                Console.WriteLine("5. Transfer Amount");
                Console.WriteLine("6. List All Accounts");
                Console.WriteLine("7. Search Account by Customer Name");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");

                int choice;

                try
                {
                    choice = int.Parse(Console.ReadLine() ?? "");
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Please enter a number from 1 to 8.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddAccount();
                        break;

                    case 2:
                        DepositMoney();
                        break;

                    case 3:
                        WithdrawMoney();
                        break;

                    case 4:
                        ShowBalance();
                        break;

                    case 5:
                        TransferAmount();
                        break;

                    case 6:
                        ListAllAccounts();
                        break;

                    case 7:
                        SearchAccountByName();
                        break;

                    case 8:
                        exitApp = true;
                        Console.WriteLine("Thank you for banking with Spark Bank. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option, please choose between 1 and 8.");
                        break;
                }
            }
        }

        static void AddAccount()
        {
            Console.WriteLine("\n--- Add New Account ---");

            Console.Write("Enter customer name: ");
            string customerName = (Console.ReadLine() ?? "").Trim();

            if (customerName == "")
            {
                Console.WriteLine("Customer name cannot be empty.");
                return;
            }

            Console.Write("Enter new account number: ");
            string accountNumber = (Console.ReadLine() ?? "").Trim();

            if (accountNumber == "")
            {
                Console.WriteLine("Account number cannot be empty.");
                return;
            }

            if (accountNumbers.Contains(accountNumber))
            {
                Console.WriteLine("Error: This account number already exists.");
                return;
            }

            Console.Write("Enter initial deposit amount: ");

            double initialDeposit;

            try
            {
                initialDeposit = double.Parse(Console.ReadLine() ?? "");
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
                return;
            }

            if (initialDeposit < 0)
            {
                Console.WriteLine("Initial deposit cannot be negative.");
                return;
            }

            customerNames.Add(customerName);
            accountNumbers.Add(accountNumber);
            balances.Add(initialDeposit);

            Console.WriteLine("\nAccount created successfully!");
            Console.WriteLine("Customer Name: " + customerName);
            Console.WriteLine("Account Number: " + accountNumber);
            Console.WriteLine("Starting Balance: " + initialDeposit);
        }

        static void DepositMoney()
        {
            Console.WriteLine("\n--- Deposit Money ---");

            Console.Write("Enter account number: ");
            string accountNumber = (Console.ReadLine() ?? "").Trim();

            int index = accountNumbers.IndexOf(accountNumber);

            if (index == -1)
            {
                Console.WriteLine("Error: Account number not found.");
                return;
            }

            Console.Write("Enter deposit amount: ");

            double depositAmount;

            try
            {
                depositAmount = double.Parse(Console.ReadLine() ?? "");
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
                return;
            }

            if (depositAmount <= 0)
            {
                Console.WriteLine("Deposit amount must be positive.");
                return;
            }

            balances[index] = balances[index] + depositAmount;

            Console.WriteLine("Deposit successful!");
            Console.WriteLine("Updated Balance: " + balances[index]);
        }

        static void WithdrawMoney()
        {
            Console.WriteLine("\n--- Withdraw Money ---");

            Console.Write("Enter account number: ");
            string accountNumber = (Console.ReadLine() ?? "").Trim();

            int index = accountNumbers.IndexOf(accountNumber);

            if (index == -1)
            {
                Console.WriteLine("Error: Account number not found.");
                return;
            }

            Console.Write("Enter withdrawal amount: ");

            double withdrawalAmount;

            try
            {
                withdrawalAmount = double.Parse(Console.ReadLine() ?? "");
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
                return;
            }

            if (withdrawalAmount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be positive.");
                return;
            }

            if (withdrawalAmount > balances[index])
            {
                Console.WriteLine("Error: Insufficient balance.");
                Console.WriteLine("Current Balance: " + balances[index]);
                return;
            }

            balances[index] = balances[index] - withdrawalAmount;

            Console.WriteLine("Withdrawal successful!");
            Console.WriteLine("Updated Balance: " + balances[index]);
        }

        static void ShowBalance()
        {
            Console.WriteLine("\n--- Show Balance ---");

            Console.Write("Enter account number: ");
            string accountNumber = (Console.ReadLine() ?? "").Trim();

            int index = accountNumbers.IndexOf(accountNumber);

            if (index == -1)
            {
                Console.WriteLine("Error: Account number not found.");
                return;
            }

            Console.WriteLine("\nAccount Details");
            Console.WriteLine("Customer Name: " + customerNames[index]);
            Console.WriteLine("Account Number: " + accountNumbers[index]);
            Console.WriteLine("Current Balance: " + balances[index]);
        }

        static void TransferAmount()
        {
            Console.WriteLine("\n--- Transfer Amount ---");

            Console.Write("Enter sender account number: ");
            string senderAccount = (Console.ReadLine() ?? "").Trim();

            Console.Write("Enter receiver account number: ");
            string receiverAccount = (Console.ReadLine() ?? "").Trim();

            int senderIndex = accountNumbers.IndexOf(senderAccount);
            int receiverIndex = accountNumbers.IndexOf(receiverAccount);

            if (senderIndex == -1)
            {
                Console.WriteLine("Error: Sender account number not found.");
                return;
            }

            if (receiverIndex == -1)
            {
                Console.WriteLine("Error: Receiver account number not found.");
                return;
            }

            if (senderIndex == receiverIndex)
            {
                Console.WriteLine("Error: Sender and receiver cannot be the same account.");
                return;
            }

            Console.Write("Enter transfer amount: ");

            double transferAmount;

            try
            {
                transferAmount = double.Parse(Console.ReadLine() ?? "");
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
                return;
            }

            if (transferAmount <= 0)
            {
                Console.WriteLine("Transfer amount must be positive.");
                return;
            }

            if (transferAmount > balances[senderIndex])
            {
                Console.WriteLine("Error: Insufficient balance.");
                Console.WriteLine("Sender Current Balance: " + balances[senderIndex]);
                return;
            }

            balances[senderIndex] = balances[senderIndex] - transferAmount;
            balances[receiverIndex] = balances[receiverIndex] + transferAmount;

            Console.WriteLine("Transfer successful!");
            Console.WriteLine("Sender Updated Balance: " + balances[senderIndex]);
            Console.WriteLine("Receiver Updated Balance: " + balances[receiverIndex]);
        }

        static void ListAllAccounts()
        {
            Console.WriteLine("\n--- List All Accounts ---");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("No accounts found.");
                return;
            }

            for (int i = 0; i < accountNumbers.Count; i++)
            {
                Console.WriteLine("\nAccount " + (i + 1));
                Console.WriteLine("Customer Name: " + customerNames[i]);
                Console.WriteLine("Account Number: " + accountNumbers[i]);
                Console.WriteLine("Balance: " + balances[i]);
            }
        }

        static void SearchAccountByName()
        {
            Console.WriteLine("\n--- Search Account by Customer Name ---");

            Console.Write("Enter customer name to search: ");
            string searchName = (Console.ReadLine() ?? "").Trim().ToLower();

            if (searchName == "")
            {
                Console.WriteLine("Search name cannot be empty.");
                return;
            }

            bool found = false;

            for (int i = 0; i < customerNames.Count; i++)
            {
                if (customerNames[i].ToLower().Contains(searchName))
                {
                    Console.WriteLine("\nAccount Found");
                    Console.WriteLine("Customer Name: " + customerNames[i]);
                    Console.WriteLine("Account Number: " + accountNumbers[i]);
                    Console.WriteLine("Balance: " + balances[i]);

                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No account found with that customer name.");
            }
        }
    }
}
