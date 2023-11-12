using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ATM_TestTask
{
    internal class PullViewModel : OperationViewModel
    {
        private ICommand _getCashAuto;
        public PullViewModel()
        {
            operation = Operation.Pull;
        }
        public override void PerformOperation()
        {
            Atm.Banknotes.ForEach(banknote => banknote.Count -= BanknotesToOperate.FirstOrDefault(bill => bill.Currency == banknote.Currency).Count);
        }
        public ICommand GetCashAuto
        {
            get => _getCashAuto ??= new CommonCommand<object>(
                (obj) =>
                {
                    CanApply = true;
                    foreach (Banknote banknote in Atm.Banknotes.OrderByDescending(banknote => banknote.Currency).ToList())
                    {
                        int i = 0;
                        while (CurrentSum + banknote.Currency <= OverallSum && i < banknote.CountDifference(operation))
                        {
                            CurrentSum += banknote.Currency;
                            i++;
                            BanknotesToOperate.First(bill => bill.Currency == banknote.Currency).Count++;
                        }
                    }
                }
                );
        }
    }
}
