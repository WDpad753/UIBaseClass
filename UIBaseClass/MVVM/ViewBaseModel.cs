using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UIBaseClass.MVVM
{
    //public class ViewModelBase : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler? PropertyChanged;

    //    public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
    //    }
    //}
    
    public class ViewModelBase : BindableBase
    {
        //public virtual void OnNavigatedTo<TParam>(TParam parameter = default) { }
        public virtual void OnNavigatedTo() { }

        public virtual void OnNavigatedFrom() { }

        //public virtual bool IsNavigationTarget<TParam>(TParam parameter) => true;
        public virtual bool IsNavigationTarget() => true;
    }
}
