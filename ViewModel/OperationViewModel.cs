using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATM_TestTask
{
    internal class OperationViewModel : ViewModelBase
    {
        private ATM _atm;
        public ATM Atm { get { return _atm; } set { _atm = value; OnPropertyChanged(nameof(Atm)); } }
        private bool _isUserInputed;
        private bool _canApply;
        private bool _canAbort;
        private int _currentSum;
        private int _overallSum;
        private string _userInput;
        private ICommand _increaseCommand;
        private ICommand _decreaseCommand;
        private ObservableCollection<Banknote> banknotesToOperate;
        public ObservableCollection<Banknote> BanknotesToOperate { get { return banknotesToOperate; } set { banknotesToOperate = value; OnPropertyChanged(nameof(BanknotesToOperate)); } }
        public string UserInput { get { return _userInput; } set { _userInput = value; OnPropertyChanged(nameof(UserInput)); } }
        public bool CanApply { get { return _canApply; } set { _canApply = value; OnPropertyChanged(nameof(CanApply)); } }
        public bool CanAbort { get { return _canAbort; } set { _canAbort = value; OnPropertyChanged(nameof(CanAbort)); } }
        public int CurrentSum { get { return _currentSum; } set { _currentSum = value; OnPropertyChanged(nameof(CurrentSum)); } }
        public int OverallSum { get { return _overallSum; } set { _overallSum = value; OnPropertyChanged(nameof(OverallSum)); } }
        public Operation operation;

        public bool IsUserInputed { get { return _isUserInputed; } set { _isUserInputed = value; OnPropertyChanged(nameof(IsUserInputed)); OnPropertyChanged(nameof(IsNotUserInputed)); } }
        public bool IsNotUserInputed { get { return !_isUserInputed; } }
        public virtual void SetPossibility(Operation operation)
        {
            foreach (Banknote banknote in BanknotesToOperate)
            {
                banknote.CanOperate = banknote.CanPerformOperation(OverallSum, CurrentSum, operation);
                banknote.CountDiff = banknote.CountDifference(operation);
            }
        }
        public virtual void PerformOperation()
        {

        }
        protected OperationViewModel()
        {
            UserInput = "";
            Atm = ATM.GetInstance(500);
            BanknotesToOperate = new ObservableCollection<Banknote>();
            Banknote.AllPossibleCurrencies.ForEach(note => BanknotesToOperate.Add(new Banknote(note)));
            CanApply = true;
            CanAbort = true;
        }

        public ICommand IncreaseCommand
        {
            get
            {
                _increaseCommand ??= new CommonCommand<object>(o => {
                    Banknote bill = o as Banknote;
                    bill.Count += 1;
                    CurrentSum += bill.Currency;
                    SetPossibility(operation);
                    CanApply = (CurrentSum == OverallSum);
                    CanAbort = (CurrentSum == 0);
                });
                return _increaseCommand;
            }
        }
        public ICommand DecreaseCommand
        {
            get
            {
                _decreaseCommand ??= new CommonCommand<object>(o => {
                    Banknote bill = o as Banknote;
                    bill.Count -= 1;
                    CurrentSum -= bill.Currency;
                    SetPossibility(operation);
                    CanApply = (CurrentSum == OverallSum);
                    CanAbort = (CurrentSum == 0);
                });
                return _decreaseCommand;
            }
        }
    }
}
