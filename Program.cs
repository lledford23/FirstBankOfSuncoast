using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace FirstBankOfSuncoast
{
    class Transaction
    {
        public string Account { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
    }
    class Program
    {
        static void DisplayTransactions(List<Transaction> transactions, string accountType)
        {
            var specificTransactions = transactions.Where(transaction => transaction.Account == accountType);
            foreach (var transaction in specificTransactions)
            {
                Console.WriteLine($"{transaction.Type} of {transaction.Amount}");
            }
        }
        static int Balance(List<Transaction> transactions, string accountType)
        {
            var withdrawTransactions = transactions.Where(transaction => transaction.Account == accountType && transaction.Type == "Withdraw");
            var depositTransactions = transactions.Where(transaction => transaction.Account == accountType && transaction.Type == "Deposit");

            var sumWithdrawAmounts = withdrawTransactions.Sum(transaction => transaction.Amount);
            var sumDepositAmounts = depositTransactions.Sum(transaction => transaction.Amount);

            return sumDepositAmounts = sumWithdrawAmounts;
        }
        static void Main(string[] args)
        {
            var transactions = new List<Transaction>();
            if (File.Exists("transactions.csv"))
            {
                var fileReader = new StreamReader("transactions.csv");
                var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                transactions = csvReader.GetRecords<Transaction>().ToList();
            }


            var mainMenu = true;

            while (mainMenu)
            {
                Console.WriteLine("Deposit - (DS) Savings");
                Console.WriteLine("Deposit - (DC) Checking");
                Console.WriteLine("Withdraw - (WS) Savings");
                Console.WriteLine("Withdraw - (WC) Checking");
                Console.WriteLine("Transfer - (T) Transfer");
                Console.WriteLine("Show History - (TS) History of Transactions in Savings");
                Console.WriteLine("Show History - (TC) History of Transations in Checking");
                Console.WriteLine("Show Balances - (BC) Checking Balance");
                Console.WriteLine("Show Balances - (BS) Savings Balance");
                Console.WriteLine("Exit - (E) Exit");
                Console.WriteLine("What would you like to do today?");
                var choice = Console.ReadLine();

                switch (choice.ToUpper())
                {
                    case "E":
                        mainMenu = false;
                        break;

                    case "DS":
                        Console.Write("How much would you like to deposit into your savings?:");
                        var depositSavings = int.Parse(Console.ReadLine());
                        if (depositSavings < 0)
                        {
                            Console.WriteLine("You must have a positive number");
                        }
                        else
                        {
                            var newTransaction = new Transaction()
                            {
                                Type = "Deposit",
                                Account = "Savings",
                                Amount = depositSavings
                            };
                            transactions.Add(newTransaction);
                        }

                        break;

                    case "DC":
                        Console.Write("How much would you like to deposit into your Checking?:");
                        var depositChecking = int.Parse(Console.ReadLine());
                        if (depositChecking < 0)
                        {
                            Console.WriteLine("You must have a positive number");
                        }
                        else
                        {
                            var newTransaction = new Transaction()
                            {
                                Type = "Deposit",
                                Account = "Checking",
                                Amount = depositChecking
                            };
                            transactions.Add(newTransaction);
                        }

                        break;

                    case "WS":
                        Console.Write("How much would you like to withdraw from your savings?:");
                        var withdrawSavings = int.Parse(Console.ReadLine());
                        if (withdrawSavings < 0)
                        {
                            Console.WriteLine("You must have a positive number");
                        }
                        else
                        {
                            var savingsBalance = Balance(transactions, "Savings");
                            if (withdrawSavings > savingsBalance)
                            {
                                Console.WriteLine("Not enough funds");
                            }
                            else
                            {
                                var newTransaction = new Transaction()
                                {
                                    Type = "Withdraw",
                                    Account = "Savings",
                                    Amount = withdrawSavings
                                };
                                transactions.Add(newTransaction);
                            }
                        }
                        break;

                    case "WC":
                        Console.Write("How much would you like to withdraw from your Checking?:");
                        var withdrawChecking = int.Parse(Console.ReadLine());
                        if (withdrawChecking < 0)
                        {
                            Console.WriteLine("You must have a positive number");
                        }
                        else
                        {
                            var checkingBalance = Balance(transactions, "Checking");
                            if (withdrawChecking > checkingBalance)
                            {
                                Console.WriteLine("Not enough funds");
                            }
                            else
                            {
                                var newTransaction = new Transaction()
                                {
                                    Type = "Withdraw",
                                    Account = "Checking",
                                    Amount = withdrawChecking
                                };
                                transactions.Add(newTransaction);
                            }
                        }
                        break;

                    case "T":
                        break;

                    case "TS":
                        DisplayTransactions(transactions, "Savings");
                        break;

                    case "TC":
                        DisplayTransactions(transactions, "Checking");
                        break;

                    case "BC":
                        var totalChecking = 0;
                        foreach (var transaction in transactions)
                        {
                            if (transaction.Account == "Checking")
                            {
                                totalChecking = totalChecking + transaction.Amount;
                            }
                        }
                        Console.WriteLine($"The balance of your Checking is {totalChecking}");
                        break;

                    case "SB":
                        var totalSavings = 0;
                        foreach (var transaction in transactions)
                        {
                            if (transaction.Account == "Savings")
                            {
                                totalSavings = totalSavings + transaction.Amount;
                            }
                        }
                        Console.WriteLine($"The balance of your Savings is {totalSavings}");
                        break;

                    default:
                        Console.WriteLine($"{choice} isn't valid. Please re-choose");
                        break;
                }

                var fileWriter = new StreamWriter("transactions.csv");
                var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(transactions);

                fileWriter.Close();

            }
        }
    }
}

