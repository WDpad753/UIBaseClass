using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDialogservice = Prism.Dialogs.IDialogService;

namespace UIBaseClass.Services.Dialog.Interface
{
    public interface IDialogService : IDialogservice
    {
        void ShowDialog(string name, IDialogParameters parameters, DialogCallback callback);
    }
}
