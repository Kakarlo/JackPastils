using JackPastil.View.InventoryChild;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JackPastil.MVVM {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        // Can be called without sending a parameter
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static implicit operator ViewModelBase(SalesPanel v) {
            throw new NotImplementedException();
        }
    }
}
