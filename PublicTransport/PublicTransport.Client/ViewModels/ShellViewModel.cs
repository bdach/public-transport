using System;
using System.Linq;
using System.Reactive.Linq;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Providers;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class ShellViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Shell";
        public IScreen HostScreen { get; }
        public MenuViewModel MenuViewModel { get; set; }
        private readonly RoutableViewModelFactory _viewModelFactory;
        private readonly PlaceholderViewModel _placeholder;

        public ShellViewModel(IScreen screen, RoutableViewModelFactory factory)
        {
            HostScreen = screen;
            _viewModelFactory = factory;
            _placeholder = new PlaceholderViewModel(screen);
            HostScreen.Router.Navigate.Execute(_placeholder);
            MenuViewModel = new MenuViewModel();

            this.WhenAny(vm => vm.MenuViewModel.SelectedOption, mvm => mvm.Value)
                .Where(e => e != null)
                .Subscribe(e =>
                {
                    if (e.Item.Option.ToString() == HostScreen.Router.NavigationStack.Last().UrlPathSegment) return;
                    var viewModel = _viewModelFactory.GetViewModel(HostScreen, e.Item.Option);
                    HostScreen.Router.NavigateAndReset.Execute(_placeholder);
                    HostScreen.Router.Navigate.Execute(viewModel);
                });

            HostScreen.Router.CurrentViewModel.Subscribe(cvm =>
            {
                var detailViewModel = cvm as IDetailViewModel;
                if (detailViewModel == null) return;
                MenuViewModel.SelectedOption =
                    MenuViewModel.Menu.FirstOrDefault(item => item.Item.Option == detailViewModel.AssociatedMenuOption);
            });
        }
    }
}