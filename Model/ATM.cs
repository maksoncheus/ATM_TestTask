using System.Collections.Generic;
using System.Linq;

namespace ATM_TestTask
{
    /// <summary>
    /// Модель для банкомата
    /// </summary>
    internal class ATM : ViewModelBase
    {
        private static ATM Instance;
        private List<Banknote> _bankNotes;
        private int _overAllCash;
        public List<Banknote> Banknotes { get { return _bankNotes; } set { _bankNotes = value; OnPropertyChanged(nameof(Banknotes)); } }
        public int MaxBanknotes { get; set; } //Максимальное количество купюр одного номинала.
        public int OverAllCash { get { return _overAllCash; } set { _overAllCash = value; OnPropertyChanged(nameof(OverAllCash)); } }
        protected ATM(int maxBanknotes)
        {
            //Не указано, что нужно хранить состояние, поэтому хардкод
            Banknotes = new();
            Banknote.AllPossibleCurrencies.ForEach(note => Banknotes.Add(new Banknote(note,maxBanknotes/2)));
            MaxBanknotes = maxBanknotes;
            RecalculateCash();
        }
        public static ATM GetInstance(int MaxBanknotes) => Instance ??= new ATM(MaxBanknotes);
        public bool IsEnoughBanknotes(int input, Operation operation)
        {
            int tempSum = 0;
            foreach (Banknote banknote in Banknotes.OrderByDescending(banknote => banknote.Currency).ToList())
            {
                int i = 0;
                while (tempSum + banknote.Currency <= input && i < banknote.CountDifference(operation))
                {
                    tempSum += banknote.Currency;
                    i++;
                }
            }
            return tempSum == input;
        }
        public void RecalculateCash() => OverAllCash = Banknotes.Sum(banknote => banknote.Currency * banknote.Count);
    }
}
