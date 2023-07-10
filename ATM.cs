using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_TestTask
{
    /// <summary>
    /// Модель для банкомата
    /// </summary>
    internal class ATM
    {
        public Dictionary<string, int> Banknotes { get; set; } // Первый - номинал, второй - количество
        int MaxBanknotes { get; set; }
        int currentBanknotes { get { return Banknotes.Values.Sum(); } }
        public ATM(int maxBanknotes)
        {
            //Не указано, что нужно хранить состояние, поэтому хардкод
            Banknotes = new Dictionary<string, int>() { { "10", 50 }, { "50", 50 }, { "100", 50 }, { "200", 50 }, { "500", 50 }, { "1000", 50 }, { "2000", 50 }, { "5000", 50 } };
            MaxBanknotes = maxBanknotes;
            int a = currentBanknotes;
        }
    }
}
