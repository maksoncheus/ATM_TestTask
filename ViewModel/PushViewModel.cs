using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ATM_TestTask
{
    internal class PushViewModel : OperationViewModel
    {
        private ICommand _increaseCommand;
        private ICommand _decreaseCommand;
        public PushViewModel()
        {
            operation = Operation.Push;
            IsUserInputed = true;
            OverallSum = Atm.Banknotes.Sum(x => Atm.MaxBanknotes * x.Currency) - Atm.OverAllCash;
            SetPossibility(operation);
        }
        public override void PerformOperation()
        {
            Atm.Banknotes.ForEach(banknote => banknote.Count += BanknotesToOperate.FirstOrDefault(bill => bill.Currency == banknote.Currency).Count);
        }
        public override ICommand IncreaseCommand
        {
            get
            {
                _increaseCommand ??= new CommonCommand<object>(o => {
                    Banknote bill = o as Banknote;
                    bill.Count += 1;
                    CurrentSum += bill.Currency;
                    SetPossibility(operation);
                    CanApply = CurrentSum > 0; //(CurrentSum == OverallSum);
                    CanAbort = (CurrentSum == 0);
                });
                return _increaseCommand;
            }
        }
        public override ICommand DecreaseCommand
        {
            get
            {
                _decreaseCommand ??= new CommonCommand<object>(o => {
                    Banknote bill = o as Banknote;
                    bill.Count -= 1;
                    CurrentSum -= bill.Currency;
                    SetPossibility(operation);
                    CanApply = CurrentSum > 0;  //(CurrentSum == OverallSum);
                    CanAbort = (CurrentSum == 0);
                });
                return _decreaseCommand;
            }
        }
    }
}
