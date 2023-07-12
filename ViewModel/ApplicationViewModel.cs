using System;
using System.Windows;
using System.Windows.Input;

namespace ATM_TestTask
{
    internal class ApplicationViewModel : OperationViewModel
    {
        private ICommand _addToUserInput;
        private ICommand _removeFromUserInput;
        private ICommand _applyUserInput;
        private bool _isNotPerformingOperation;

        private OperationViewModel _currentPageViewModel;
        public ApplicationViewModel()
        {
            CurrentPageViewModel = new MainPageViewModel();
            IsNotPerformingOperation = true;
        }
        public bool IsNotPerformingOperation { get { return _isNotPerformingOperation; } set { _isNotPerformingOperation = value; OnPropertyChanged(nameof(IsNotPerformingOperation)); } }
        public ICommand AddToUserInput
        {
            get
            {
                
                _addToUserInput ??= new CommonCommand<object>(
                        o => { if (CurrentPageViewModel is not MainPageViewModel) CurrentPageViewModel.UserInput += (o as string); OnPropertyChanged(nameof(CurrentPageViewModel)); }
                        ) ;

                return _addToUserInput;
            }
        }
        public ICommand RemoveFromUserInput
        {
            get
            {
                _removeFromUserInput ??= new CommonCommand(
                        () => { if (CurrentPageViewModel.UserInput.Length>0) CurrentPageViewModel.UserInput = CurrentPageViewModel.UserInput.Remove(CurrentPageViewModel.UserInput.Length - 1); OnPropertyChanged(nameof(CurrentPageViewModel)); }
                        );

                return _removeFromUserInput;
            }
        }
        public ICommand ApplyUserInput
        {
            get
            {
                return _applyUserInput ??= new CommonCommand<object>(o =>
                {
                    if (CurrentPageViewModel.IsNotUserInputed)
                    {
                        if (o != null)
                        {
                            string input = (o as string);
                            if (input.Length > 0 && input.Length < 7 && !input.StartsWith('0'))
                            {
                                int UserInputValue = Convert.ToInt32(input);
                                if (!Atm.IsEnoughBanknotes(UserInputValue, CurrentPageViewModel.operation)) MessageBox.Show(CurrentPageViewModel.operation switch { Operation.Push => "В банкомате не хватает места для купюр на эту сумму.", Operation.Pull => "В банкомате не хватает купюр на эту сумму." });
                                else
                                {
                                    CurrentPageViewModel.IsUserInputed = true;
                                    CurrentPageViewModel.OverallSum = UserInputValue;
                                    CurrentPageViewModel.CurrentSum = 0;
                                    CurrentPageViewModel.SetPossibility(CurrentPageViewModel.operation);
                                    CurrentPageViewModel.UserInput = "";
                                    CurrentPageViewModel.CanApply = (CurrentPageViewModel.CurrentSum == CurrentPageViewModel.OverallSum);
                                }
                            }
                            else MessageBox.Show("Недопустимое значение!");
                        }
                    }
                    else
                        if (CurrentPageViewModel.CanApply)
                    {
                        CurrentPageViewModel.PerformOperation();
                        MessageBox.Show("Вы " + CurrentPageViewModel.operation switch { Operation.Push => "внесли", Operation.Pull => "сняли" } + $" {CurrentPageViewModel.OverallSum} рублей");
                        Atm.RecalculateCash();
                        ChangeToMain?.Execute(null);
                    }
                });
            }
        }
        public ICommand ChangeToPush
        {
            get { return new CommonCommand(() => { CurrentPageViewModel = new PushViewModel(); IsNotPerformingOperation = false; }); }
        }
        public ICommand ChangeToPull
        {
            get { return new CommonCommand(() => { CurrentPageViewModel = new PullViewModel(); IsNotPerformingOperation = false; }); }
        }
        public ICommand ChangeToMain
        {
            get { return new CommonCommand(() => { CurrentPageViewModel = new MainPageViewModel(); IsNotPerformingOperation = true; }); }
        }
        public OperationViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }
    }
}
