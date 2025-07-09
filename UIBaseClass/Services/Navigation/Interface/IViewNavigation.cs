using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIBaseClass.Services.Navigation.Interface
{
    public interface IViewNavigation
    {
        //void NavigateTo<TViewModel, TParam>(TParam parameter = default);
        void NavigateTo<TViewModel>();
    }
}
