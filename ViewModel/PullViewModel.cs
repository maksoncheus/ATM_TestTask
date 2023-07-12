using System.Collections.ObjectModel;
using System.Linq;

namespace ATM_TestTask
{
    internal class PullViewModel : OperationViewModel
    {
        public PullViewModel()
        {
            operation = Operation.Pull;
        }
        public override void PerformOperation()
        {
            Atm.Banknotes.ForEach(banknote => banknote.Count -= BanknotesToOperate.FirstOrDefault(bill => bill.Currency == banknote.Currency).Count);
        }
    }
}
