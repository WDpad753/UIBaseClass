using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDialogService = UIBaseClass.Services.Dialog.Interface.IDialogService;

namespace UIBaseClass.Services.Dialog
{
    public class DialogService : IDialogService
    {
        private readonly IContainerExtension _container;

        public DialogService(IContainerExtension container)
        {
            _container = container;
        }

        public void ShowDialog(string name, IDialogParameters parameters, DialogCallback callback)
        {
            var dialogWindow = CreateDialogWindow();
            var content = _container.Resolve<object>(name);

            if (content is not IDialogAware viewModel)
                throw new NullReferenceException("Dialog content must implement IDialogAware.");

            dialogWindow.DataContext = viewModel;
            viewModel.OnDialogOpened(parameters);

            dialogWindow.Closed += (o, e) =>
            {
                callback.Invoke(new DialogResult(ButtonResult.OK));
            };

            dialogWindow.ShowDialog();
        }

        private IDialogWindow CreateDialogWindow()
        {
            var window = new DialogWindow();
            return window;
        }
    }
}
