using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_TestTask
{
    internal class Banknote : ViewModelBase
    {
        public static List<int> AllPossibleCurrencies = new() { 10, 50, 100, 200, 500, 1000, 2000, 5000 };
        private int _count;
        private int _countDiff;
        private bool _canOperate = true;
        public int Currency { get; }
        public int Count { get { return _count; } set { _count = value; OnPropertyChanged(nameof(Count)); } }
        public bool CanOperate { get { return _canOperate; } set { _canOperate = value; OnPropertyChanged(nameof(CanOperate)); } }
        public int CountDiff { get { return _countDiff; } set { _countDiff = value; OnPropertyChanged(nameof(CountDiff)); } }

        public Banknote(int currency, int count)
        {
            if (!AllPossibleCurrencies.Contains(currency)) throw new ArgumentException("Вы пытаетесь добавить банкноту несуществующего номинала!", nameof(currency));
            Currency = currency;
            Count = count;
        }
        public Banknote(int currency) : this(currency, 0) { }
        public bool CanPerformOperation(int overallSum, int currentSum, Operation operation) => Currency + currentSum <= overallSum && Count < CountDifference(operation);
        public int CountDifference(Operation op) => Math.Abs(op switch { Operation.Push => ATM.GetInstance(0).MaxBanknotes, Operation.Pull => 0 } - ATM.GetInstance(0).Banknotes.FirstOrDefault(note => note.Currency == Currency).Count);
        /*{
                if (Count * Convert.ToInt32(Currency) + currentSum <= overallSum) return Count;
                else return (overallSum - currentSum) / Convert.ToInt32(Currency);
        }*/
    }
}
