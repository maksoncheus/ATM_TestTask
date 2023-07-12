namespace ATM_TestTask
{
    internal class MainPageViewModel : OperationViewModel
    {
        public MainPageViewModel()
        {
            CanApply = false;
            CanAbort = false;
            IsUserInputed = true;
        }
    }
}
