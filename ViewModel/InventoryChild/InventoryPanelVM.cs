using JackPastil.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JackPastil.ViewModel {
    public class InventoryPanelVM : ViewModelBase {

        private string _testing;

        public InventoryPanelVM() { 
        }

        public string Testing { get => _testing; set { _testing = value; OnPropertyChanged(); MessageBox.Show(_testing); } }
    }
}
