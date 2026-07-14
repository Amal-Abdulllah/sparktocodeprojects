using System;
using System.Linq;

namespace OPPtask1
{
    // ======================================================================
    //  CLASS: BankAccount
    // ======================================================================
    class BankAccount
    {
        // ---- Public properties (attributes) ----
        public int AccountNumber;
        public string HolderName;
        public double Balance;

        // Parameterless constructor.
        // Required because adding the parameterized constructor below removes
        // C#'s automatic default constructor - Case 1-15 objects still need
        // to be created with "new BankAccount()" and set field-by-field.
        public BankAccount()
        {
        }

        // Case 16 - Parameterized constructor: builds a fully-formed account
        // in one call, with no separate property assignments afterward.
        public BankAccount(int accountNumber, string holderName, double balance)
        {
            AccountNumber = accountNumber;
            HolderName = holderName;
            Balance = balance;
        }

        // Adds the amount, then always notifies (per spec).
        public void Deposit(double amount)
        {
            Balance += amount;
            SendEmail();
        }

        // Only withdraws if funds are sufficient; either way, notifies.
        public void Withdraw(double amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
            }
            else
            {
                Console.WriteLine("Withdrawal denied: insufficient balance.");
            }
            SendEmail();
        }

        // Public "read" entry point - shows the info, then hands back the value.
        public double CheckBalance()
        {
            PrintInformation();
            return Balance;
        }

        // Case 18 - Read-only property (get accessor only).
        // No "set" exists, so this can never be assigned from outside the class.
        public bool IsOverdrawn
        {
            get { return Balance < 0; }
        }

        // private - only reachable from inside this class (e.g. via CheckBalance()).
        private void PrintInformation()
        {
            Console.WriteLine($"    Account #{AccountNumber} | Holder: {HolderName} | Balance: {Balance:F2}");
        }

        // private placeholder - no real email sending, just simulates the behavior.
        private void SendEmail()
        {
            Console.WriteLine("    [Email notification sent to account holder.]");
        }
    }

    // ======================================================================
    //  CLASS: Student
    // ======================================================================
    class Student
    {
        // ---- Public properties ----
        public int Grade;
        public string Name;
        public string Address;

        // ---- Encapsulated fields: only touchable from inside this class ----
        private string email;   // private

        // default access = private within this class.
        // Required by the spec as a field with default access, but no case
        // (1-19) actually reads or writes it, so the compiler would flag it
        // as unused. Suppressed deliberately rather than inventing a fake use.
#pragma warning disable CS0169
        int age;
#pragma warning restore CS0169

        // Case 17 - static field: shared by the class, not by each instance.
        private static int studentCount = 0;

        // private backing field for Case 19's write-only property.
        private string pin = "";

        public Student()
        {
            studentCount++;   // every new Student increases the shared counter
        }

        // The ONLY way to set 'email' from outside the class.
        public void Register(string Email)
        {
            email = Email;
            SendEmail();
        }

        // Case 19 - Write-only property (set accessor only, no get).
        // Once set, the PIN cannot be read back from outside the class.
        public string SecurityPIN
        {
            set { pin = value; }
        }

        // Case 17 - static method: called through the class name (Student.GetStudentCount()),
        // not through any particular object.
        public static int GetStudentCount()
        {
            return studentCount;
        }

        // private - only reachable from inside this class (e.g. via Register()).
        private void SendEmail()
        {
            Console.WriteLine("    [Registration email sent to student.]");
        }
    }

    // ======================================================================
    //  CLASS: Product
    // ======================================================================
    class Product
    {
        // ---- Public properties ----
        public string ProductName;
        public double Price;
        public int StockQuantity;

        // Only reduces stock if enough is available; either way, logs the transaction.
        public void Sell(int quantity)
        {
            if (quantity <= StockQuantity)
            {
                StockQuantity -= quantity;
            }
            else
            {
                Console.WriteLine("    Not enough stock to complete this sale.");
            }
            LogTransaction();
        }

        // Always succeeds; adds stock, then logs the transaction.
        public void Restock(int quantity)
        {
            StockQuantity += quantity;
            LogTransaction();
        }

        // Public "read" entry point - shows details, then hands back the computed value.
        public double GetInventoryValue()
        {
            PrintDetails();
            return Price * StockQuantity;
        }

        // private - only reachable from inside this class (e.g. via GetInventoryValue()).
        private void PrintDetails()
        {
            Console.WriteLine($"    Product: {ProductName} | Price: {Price:F2} | Stock: {StockQuantity}");
        }

        // private placeholder - no real logging, just simulates the behavior.
        private void LogTransaction()
        {
            Console.WriteLine("    [Transaction logged.]");
        }
    }

    // ======================================================================
    //  PROGRAM - menu loop + the six required objects (no collections)
    // ======================================================================
    class Program
    {
        // Exactly two of each - individually named fields, never an array/List.
        static BankAccount account1 = new BankAccount();
        static BankAccount account2 = new BankAccount();
        static Student student1 = new Student();
        static Student student2 = new Student();
        static Product product1 = new Product();
        static Product product2 = new Product();

        static void Main(string[] args)
        {
            SeedStartingData();

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (choice)
                {
                    case "1": Case1_ViewAccountDetails(); break;
                    case "2": Case2_UpdateStudentAddress(); break;
                    case "3": Case3_MakeDeposit(); break;
                    case "4": Case4_MakeWithdrawal(); break;
                    case "5": Case5_ViewProductDetails(); break;
                    case "6": Case6_RegisterStudent(); break;
                    case "7": Case7_CompareBalances(); break;
                    case "8": Case8_RestockAndCheckLevel(); break;
                    case "9": Case9_TransferBetweenAccounts(); break;
                    case "10": Case10_UpdateGradeValidated(); break;
                    case "11": Case11_StudentReportCard(); break;
                    case "12": Case12_AccountHealthStatus(); break;
                    case "13": Case13_BulkSaleWithRevenue(); break;
                    case "14": Case14_ScholarshipEligibility(); break;
                    case "15": Case15_FullBalanceTopUp(); break;
                    case "16": Case16_QuickAccountOpening(); break;
                    case "17": Case17_TotalStudentsCounter(); break;
                    case "18": Case18_OverdrawnCheck(); break;
                    case "19": Case19_SetStudentPin(); break;
                    case "20":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 1 to 20.");
                        break;
                }

                Console.WriteLine();
            }
        }

        // Sets up the starting values using plain property assignment,
        // exactly as taught in class (no constructors involved here).
        static void SeedStartingData()
        {
            account1.AccountNumber = 1163;
            account1.HolderName = "karim";
            account1.Balance = 120;

            account2.AccountNumber = 15203;
            account2.HolderName = "Ali";
            account2.Balance = 63;

            student1.Name = "Ali";
            student1.Address = "Muscat";
            student1.Grade = 65;

            student2.Name = "Ahmed";
            student2.Address = "Muscat";
            student2.Grade = 70;

            product1.ProductName = "Wireless Mouse";
            product1.Price = 5.500;
            product1.StockQuantity = 50;

            product2.ProductName = "Mechanical Keyboard";
            product2.Price = 15.750;
            product2.StockQuantity = 20;
        }

        static void ShowMenu()
        {
            Console.WriteLine("================ MAIN MENU ================");
            Console.WriteLine(" 1. View Account Details");
            Console.WriteLine(" 2. Update Student Address");
            Console.WriteLine(" 3. Make a Deposit");
            Console.WriteLine(" 4. Make a Withdrawal");
            Console.WriteLine(" 5. View Product Details");
            Console.WriteLine(" 6. Register a Student");
            Console.WriteLine(" 7. Compare Two Account Balances");
            Console.WriteLine(" 8. Restock Product & Stock Level Check");
            Console.WriteLine(" 9. Transfer Between Accounts");
            Console.WriteLine("10. Update Student Grade (Validated)");
            Console.WriteLine("11. Student Report Card");
            Console.WriteLine("12. Account Health Status");
            Console.WriteLine("13. Bulk Sale With Revenue Calculation");
            Console.WriteLine("14. Scholarship Eligibility Check");
            Console.WriteLine("15. Full Balance Top-Up Flow");
            Console.WriteLine("16. Quick Account Opening      [Parameterized Constructor]");
            Console.WriteLine("17. Total Students Counter     [Static Field & Method]");
            Console.WriteLine("18. Overdrawn Account Check    [Read-Only Property]");
            Console.WriteLine("19. Set Student Security PIN   [Write-Only Property]");
            Console.WriteLine("20. Exit");
            Console.Write("Enter your choice: ");
        }

        // ------------------------------------------------------------
        // Selection helpers (used by many cases to ask "which one?")
        // ------------------------------------------------------------
        static BankAccount SelectAccount(string context = "an account")
        {
            Console.WriteLine($"Select {context}:");
            Console.WriteLine($"  1. Account #{account1.AccountNumber} - {account1.HolderName}");
            Console.WriteLine($"  2. Account #{account2.AccountNumber} - {account2.HolderName}");
            Console.Write("Enter 1 or 2: ");
            string input = Console.ReadLine() ?? "";
            if (input == "2") return account2;
            if (input != "1") Console.WriteLine("Unrecognized entry - defaulting to option 1.");
            return account1;
        }

        static Student SelectStudent(string context = "a student")
        {
            Console.WriteLine($"Select {context}:");
            Console.WriteLine($"  1. {student1.Name}");
            Console.WriteLine($"  2. {student2.Name}");
            Console.Write("Enter 1 or 2: ");
            string input = Console.ReadLine() ?? "";
            if (input == "2") return student2;
            if (input != "1") Console.WriteLine("Unrecognized entry - defaulting to option 1.");
            return student1;
        }

        static Product SelectProduct(string context = "a product")
        {
            Console.WriteLine($"Select {context}:");
            Console.WriteLine($"  1. {product1.ProductName}");
            Console.WriteLine($"  2. {product2.ProductName}");
            Console.Write("Enter 1 or 2: ");
            string input = Console.ReadLine() ?? "";
            if (input == "2") return product2;
            if (input != "1") Console.WriteLine("Unrecognized entry - defaulting to option 1.");
            return product1;
        }

        // ================= EASY =================

        // Case 1 - View Account Details
        static void Case1_ViewAccountDetails()
        {
            Console.WriteLine("-- Case 1: View Account Details --");
            BankAccount acc = SelectAccount();
            double balance = acc.CheckBalance();
            Console.WriteLine($"(CheckBalance() returned: {balance:F2})");
        }

        // Case 2 - Update Student Address
        static void Case2_UpdateStudentAddress()
        {
            Console.WriteLine("-- Case 2: Update Student Address --");
            Student s = SelectStudent();
            Console.Write("Enter new address: ");
            string newAddress = Console.ReadLine() ?? "";
            s.Address = newAddress;
            Console.WriteLine($"Address updated. {s.Name}'s new address is: {s.Address}");
        }

        // Case 3 - Make a Deposit
        static void Case3_MakeDeposit()
        {
            Console.WriteLine("-- Case 3: Make a Deposit --");
            BankAccount acc = SelectAccount();
            Console.Write("Enter deposit amount: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                acc.Deposit(amount);
                Console.WriteLine($"{acc.HolderName}'s updated balance: {acc.Balance:F2}");
            }
            else
            {
                Console.WriteLine("Invalid amount. Deposit cancelled.");
            }
        }

        // Case 4 - Make a Withdrawal
        static void Case4_MakeWithdrawal()
        {
            Console.WriteLine("-- Case 4: Make a Withdrawal --");
            BankAccount acc = SelectAccount();
            Console.Write("Enter withdrawal amount: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                acc.Withdraw(amount);
                Console.WriteLine($"Updated balance: {acc.Balance:F2}");
            }
            else
            {
                Console.WriteLine("Invalid amount. Withdrawal cancelled.");
            }
        }

        // Case 5 - View Product Details
        static void Case5_ViewProductDetails()
        {
            Console.WriteLine("-- Case 5: View Product Details --");
            Product p = SelectProduct();
            double value = p.GetInventoryValue();
            Console.WriteLine($"Total inventory value: {value:F2}");
        }

        // ================= MEDIUM =================

        // Case 6 - Register a Student
        static void Case6_RegisterStudent()
        {
            Console.WriteLine("-- Case 6: Register a Student --");
            Student s = SelectStudent();
            Console.Write("Enter email address: ");
            string email = Console.ReadLine() ?? "";
            s.Register(email);   // only way to reach the private 'email' field
            Console.WriteLine($"{s.Name} has been registered successfully.");
        }

        // Case 7 - Compare Two Account Balances
        static void Case7_CompareBalances()
        {
            Console.WriteLine("-- Case 7: Compare Two Account Balances --");
            if (account1.Balance > account2.Balance)
            {
                Console.WriteLine($"{account1.HolderName} holds more money ({account1.Balance:F2} vs {account2.Balance:F2}).");
            }
            else if (account2.Balance > account1.Balance)
            {
                Console.WriteLine($"{account2.HolderName} holds more money ({account2.Balance:F2} vs {account1.Balance:F2}).");
            }
            else
            {
                Console.WriteLine($"Both accounts are equal, at {account1.Balance:F2}.");
            }
        }

        // Case 8 - Restock Product & Stock Level Check
        static void Case8_RestockAndCheckLevel()
        {
            Console.WriteLine("-- Case 8: Restock Product & Stock Level Check --");
            Product p = SelectProduct();
            Console.Write("Enter quantity to restock: ");
            if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
            {
                p.Restock(qty);

                string level;
                if (p.StockQuantity < 10) level = "Low";
                else if (p.StockQuantity <= 49) level = "Moderate";
                else level = "Well Stocked";

                Console.WriteLine($"New stock: {p.StockQuantity} -> Level: {level}");
            }
            else
            {
                Console.WriteLine("Invalid quantity. Restock cancelled.");
            }
        }

        // ================= HARD =================

        // Case 9 - Transfer Between Accounts
        static void Case9_TransferBetweenAccounts()
        {
            Console.WriteLine("-- Case 9: Transfer Between Accounts --");
            BankAccount source = SelectAccount("the SOURCE account");
            BankAccount destination = SelectAccount("the DESTINATION account");
            Console.Write("Enter amount to transfer: ");

            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Transfer cancelled.");
                return;
            }

            // Check BEFORE changing anything.
            if (source.Balance >= amount)
            {
                source.Withdraw(amount);
                destination.Deposit(amount);
                Console.WriteLine($"Transfer successful. {source.HolderName}: {source.Balance:F2} | {destination.HolderName}: {destination.Balance:F2}");
            }
            else
            {
                Console.WriteLine($"Transfer failed: {source.HolderName} has insufficient funds. Both accounts left unchanged.");
            }
        }

        // Case 10 - Update Student Grade (Validated)
        static void Case10_UpdateGradeValidated()
        {
            Console.WriteLine("-- Case 10: Update Student Grade (Validated) --");
            Student s = SelectStudent();
            Console.Write("Enter new grade: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int grade))
            {
                Console.WriteLine("Invalid input: grade must be a whole number. No change made.");
                return;
            }
            if (grade < 0 || grade > 100)
            {
                Console.WriteLine("Invalid grade: must be between 0 and 100. No change made.");
                return;
            }

            s.Grade = grade;
            Console.WriteLine($"{s.Name}'s grade updated to {s.Grade}.");
        }

        // Case 11 - Student Report Card
        static void Case11_StudentReportCard()
        {
            Console.WriteLine("-- Case 11: Student Report Card --");
            Student s = SelectStudent();
            string status = s.Grade >= 60 ? "Pass" : "Fail";

            Console.WriteLine("========== REPORT CARD ==========");
            Console.WriteLine($"Name:    {s.Name}");
            Console.WriteLine($"Address: {s.Address}");
            Console.WriteLine($"Grade:   {s.Grade}");
            Console.WriteLine($"Status:  {status}");
            Console.WriteLine("==================================");
        }

        // Case 12 - Account Health Status
        static void Case12_AccountHealthStatus()
        {
            Console.WriteLine("-- Case 12: Account Health Status --");
            BankAccount acc = SelectAccount();

            string status;
            if (acc.Balance < 50) status = "Low Balance";
            else if (acc.Balance <= 1000) status = "Healthy";
            else status = "Premium";

            Console.WriteLine($"{acc.HolderName}'s account status: {status}");
        }

        // Case 13 - Bulk Sale With Revenue Calculation
        static void Case13_BulkSaleWithRevenue()
        {
            Console.WriteLine("-- Case 13: Bulk Sale With Revenue Calculation --");
            Product p = SelectProduct();
            Console.Write("Enter quantity to sell: ");

            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity. Sale cancelled.");
                return;
            }

            if (quantity > p.StockQuantity)
            {
                int shortfall = quantity - p.StockQuantity;
                Console.WriteLine($"Not enough stock. You need {shortfall} more unit(s) to fulfill this order. No sale made.");
            }
            else
            {
                p.Sell(quantity);
                double revenue = quantity * p.Price;
                Console.WriteLine($"Sale complete. Total revenue: {revenue:F2}");
            }
        }

        // ================= ADVANCED =================

        // Case 14 - Scholarship Eligibility Check
        static void Case14_ScholarshipEligibility()
        {
            Console.WriteLine("-- Case 14: Scholarship Eligibility Check --");
            Student s = SelectStudent();
            BankAccount acc = SelectAccount();

            bool gradeOk = s.Grade >= 80;
            bool balanceOk = acc.Balance >= 100;

            if (gradeOk && balanceOk)
            {
                Console.WriteLine("Result: Eligible");
            }
            else
            {
                Console.WriteLine("Result: Not Eligible");
                if (!gradeOk) Console.WriteLine($"  - Grade too low: {s.Grade} (needs 80 or above)");
                if (!balanceOk) Console.WriteLine($"  - Balance too low: {acc.Balance:F2} (needs 100 or above)");
            }
        }

        // Case 15 - Full Balance Top-Up Flow
        static void Case15_FullBalanceTopUp()
        {
            Console.WriteLine("-- Case 15: Full Balance Top-Up Flow --");
            BankAccount acc = SelectAccount();
            double before = acc.Balance;

            if (before < 50)
            {
                double topUp = 100 - before;
                acc.Deposit(topUp);
                Console.WriteLine($"Balance before: {before:F2} | Balance after: {acc.Balance:F2}");
            }
            else
            {
                Console.WriteLine($"No top-up needed - balance is already {before:F2} (50 or above).");
            }
        }

        // ================= RESEARCH-BASED =================

        // Case 16 - Quick Account Opening [Parameterized Constructor]
        static void Case16_QuickAccountOpening()
        {
            Console.WriteLine("-- Case 16: Quick Account Opening (Parameterized Constructor) --");
            Console.Write("Enter new account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accNum))
            {
                Console.WriteLine("Invalid account number. Account creation cancelled.");
                return;
            }
            Console.Write("Enter holder name: ");
            string holderName = Console.ReadLine() ?? "";
            Console.Write("Enter starting balance: ");
            if (!double.TryParse(Console.ReadLine(), out double startBalance))
            {
                Console.WriteLine("Invalid balance. Account creation cancelled.");
                return;
            }

            // Built ONLY through the parameterized constructor - no property
            // assignments afterward.
            BankAccount newAccount = new BankAccount(accNum, holderName, startBalance);

            Console.WriteLine("New account created:");
            newAccount.CheckBalance();
        }

        // Case 17 - Total Students Counter [Static Field & Method]
        static void Case17_TotalStudentsCounter()
        {
            Console.WriteLine("-- Case 17: Total Students Counter (Static) --")  ;
            // Called through the CLASS NAME, not through student1/student2.
            Console.WriteLine($"Total Student objects created so far: {Student.GetStudentCount()}");
        }

        // Case 18 - Overdrawn Account Check [Read-Only Property]
        static void Case18_OverdrawnCheck()
        {
            Console.WriteLine("-- Case 18: Overdrawn Account Check (Read-Only Property) --");
            BankAccount acc = SelectAccount();
            if (acc.IsOverdrawn)
                Console.WriteLine($"{acc.HolderName}'s account is OVERDRAWN (balance: {acc.Balance:F2}).");
            else
                Console.WriteLine($"{acc.HolderName}'s account is not overdrawn (balance: {acc.Balance:F2}).");
        }

        // Case 19 - Set Student Security PIN [Write-Only Property]
        static void Case19_SetStudentPin()
        {
            Console.WriteLine("-- Case 19: Set Student Security PIN (Write-Only Property) --");
            Student s = SelectStudent();
            Console.Write("Enter a 4-digit PIN: ");
            string input = Console.ReadLine() ?? "";

            if (input.Length == 4 && input.All(char.IsDigit))
            {
                s.SecurityPIN = input;   // set-only: cannot be read back
                Console.WriteLine("PIN set successfully. (It cannot be printed back - the property is write-only.)");
            }
            else
            {
                Console.WriteLine("Invalid PIN - it must be exactly 4 digits. No change made.");
            }
        }
    }
}