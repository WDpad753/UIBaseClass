using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using UIBaseClass.MVVM.ViewBase;
using UIBaseClass.Services.Navigation.Interface;

namespace UIBaseClass.Services.Navigation
{
    public class ViewNavigation : ViewModelBase, IViewNavigation
    {
        private readonly IContainerProvider _containerProvider;
        private readonly Func<Type, object> _viewFactory;
        private readonly Func<Type, Type> _viewResolver;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            private set => SetProperty(ref _currentView, value);
        }

        public ViewNavigation(IContainerProvider container, Func<Type, object> viewFactory, Func<Type, Type> viewResolver)
        {
            _viewFactory = viewFactory;
            _viewResolver = viewResolver;
            _containerProvider = container;
        }

        //void IViewNavigation.NavigateTo<TViewModel, TParam>(TParam parameter = default)
        void IViewNavigation.NavigateTo<TViewModel>()
        {
            var viewModelType = typeof(TViewModel);
            var baseNamespace = viewModelType.Namespace.Replace("ViewModels", "Views");
            var viewModelName = viewModelType.Name.Replace("ViewModel", "");
            var viewTypeName = $"{baseNamespace}.{viewModelName}.{viewModelName}, {viewModelType.Assembly.FullName}";
            var viewType = _viewResolver(Type.GetType(viewTypeName));
            if (viewType == null)
                throw new InvalidOperationException($"No view found for {typeof(TViewModel).Name}");

            var newView = _viewFactory(viewType) as FrameworkElement;
            if (newView == null)
                throw new InvalidOperationException($"Resolved view {viewType.Name} is not a FrameworkElement");

            // Check if current view's VM wants to be navigated from
            var currentVM = (_currentView as FrameworkElement)?.DataContext as ViewModelBase;
            currentVM?.OnNavigatedFrom();

            var newVM = newView.DataContext as ViewModelBase;
            if (newVM == null)
            {
                newVM = _containerProvider.Resolve(viewModelType) as ViewModelBase;
                newView.DataContext = newVM;
            }

            //// Ask if the target view is reusable
            //if (newVM != null && !newVM.IsNavigationTarget(parameter))
            //{
            //    // You could choose to skip navigation or force re-creation here
            //    throw new InvalidOperationException($"{newVM.GetType().Name} rejected navigation");
            //}

            //if (newView.DataContext is ViewModelBase vm)
            //{
            //    vm.OnNavigatedTo(parameter);
            //}

            //newVM?.OnNavigatedTo(parameter);
            newVM?.OnNavigatedTo();

            CurrentView = newView;
        }
    }
}
