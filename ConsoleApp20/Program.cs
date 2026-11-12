using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccount
{
    class Program
    {
        static void Main(string[] args)
        {
            int threadCount = 10;
            int depositsPerThread = 10000;
            int depositAmount = 1;
            int expectedbalance = 10000 + (threadCount * depositsPerThread * depositAmount);

            Console.WriteLine("--- Part 1: Race Condition ---");

            BankAccount account1 = new BankAccount(10000);

            Task[] tasks1 = new Task[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                tasks1[i] = Task.Run(() =>
                {
                    for (int j = 0; j < depositsPerThread; j++)
                    {
                        account1.Deposit(depositAmount);
                    }
                });
            }


            Task.WaitAll(tasks1);
            Console.WriteLine($"Expected balance: {expectedbalance}");
            Console.WriteLine($"Actual balance: {account1.GetBalance()}");

            Console.WriteLine("--- Part 2: Race Condition ---");


            BankAccount account2 = new BankAccount(10000);
            Task[] tasks2 = new Task[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                tasks2[i] = Task.Run(() =>
                {
                    for (int j = 0; j < depositsPerThread; j++)
                    {
                        account2.DepositWithLock(depositAmount);
                    }
                });
            }

            Task.WaitAll(tasks2);

            int result2 = account2.GetBalance();

            Console.WriteLine($"Expected balance: {expectedbalance}");
            Console.WriteLine($"Actual balance: {result2}");


            Console.WriteLine("\n--- Part 3: Interlocked ---");

            BankAccount account3 = new BankAccount(10000);
            Task[] tasks3 = new Task[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                tasks3[i] = Task.Run(() =>
                {
                
                    for (int j = 0; j < depositsPerThread; j++)
                    {
                        account3.DepositWithLock(depositAmount);
                    }
                });
            }
            Task.WaitAll(tasks3);


            Console.WriteLine($"Expected balance: {expectedbalance}\n");
            Console.WriteLine("--- comparison of results ---");
            Console.WriteLine($"Part 1: {account1.GetBalance()}");
            Console.WriteLine($"Part 2: {account2.GetBalance()}");
            Console.WriteLine($"Part 3: {account3.GetBalance()}");

            Console.ReadKey();
        }
        class BankAccount
        {
            private int balance;
            private object balanceLock = new object();
            public BankAccount(int initialBalance)
            {
                balance = initialBalance;
            }
            public void Deposit(int amount)
            {
                {
                    balance += amount;
                }
            }

            public void DepositWithLock(int amount)
            {
                {
                   Interlocked.Add(ref balance, amount);
                }
            }

            public void DepositInterlocked(int amount)
            {
                {
                    Interlocked.Add(ref balance, amount);
                }
            }
            public int GetBalance()
            {
                {
                    return balance;
                }
            }
        }
    }
}







