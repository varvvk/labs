using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class BankAccount
    {
        private string Name;
        private int Balance;

        public BankAccount(string name, int balance)
        {
            Name = name;
            Balance = balance;
        }

        public int Deposit(int amount)
        {
            return Balance += amount;
        }

        public void Withdraw(int amount)
        {
            if (Balance > amount)
                Balance -= amount;
            else
                Console.WriteLine("Error");
                return;
        }

        public void GetBalance()
        {
            Console.WriteLine($"{Name} balance = {Balance}");
        }
    }
}
