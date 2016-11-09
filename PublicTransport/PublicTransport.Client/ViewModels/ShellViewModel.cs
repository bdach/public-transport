using System;
using System.Linq;
using System.Reactive.Linq;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Providers;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    /// <summary>
    ///     Root view model of the application.
    /// </summary>
    public class ShellViewModel : ReactiveObject, IRoutableViewModel
    {
        /// <summary>
        ///     Placeholder view model. Displayed in the detail area upon application startup.
        /// </summary>
        private readonly PlaceholderViewModel _placeholder;

        /// <summary>
        ///     Factory used for supplying instances of <see cref="IDetailViewModel" />. Used in navigation.
        /// </summary>
        private readonly DetailViewModelFactory _viewModelFactory;

        public ShellViewModel(IScreen screen, DetailViewModelFactory factory)
        {
            #region Field/property initialization

            HostScreen = screen;
            _viewModelFactory = factory;
            _placeholder = new PlaceholderViewModel(screen);
            NotificationViewModel = new NotificationViewModel();
            MenuViewModel = new MenuViewModel();
            LoginViewModel = new LoginViewModel(screen);

            #endregion

            // Set detail area startup view model
            HostScreen.Router.Navigate.Execute(LoginViewModel);

            #region Login support

            this.WhenAnyValue(vm => vm.LoginViewModel.UserInfo)
                .Where(ui => ui != null)
                .Subscribe(ui =>
                {
                    MenuViewModel.UserInfo = ui;
                    HostScreen.Router.NavigateAndReset.Execute(_placeholder);
                });

            #endregion

            #region Logging out

            MenuViewModel.LogOut.Subscribe(_ => HostScreen.Router.Navigate.Execute(LoginViewModel));

            #endregion

            #region Detail view switching

            this.WhenAny(vm => vm.MenuViewModel.SelectedOption, mvm => mvm.Value)
                .Where(e => e != null)
                .Subscribe(e =>
                {
                    // TODO: Fix this to use the menu enum instead
                    if (e.Item.Option.ToString() == HostScreen.Router.NavigationStack.Last().UrlPathSegment)
                    {
                        return;
                    }
                    var viewModel = _viewModelFactory.GetViewModel(HostScreen, e.Item.Option);
                    HostScreen.Router.NavigateAndReset.Execute(_placeholder);
                    HostScreen.Router.Navigate.Execute(viewModel);
                });

            #endregion

            #region Updating the selected menu item upon switch caused by another view model

            HostScreen.Router.CurrentViewModel.Subscribe(cvm =>
            {
                var detailViewModel = cvm as IDetailViewModel;
                MenuViewModel.SelectedOption =
                    MenuViewModel.Menu.FirstOrDefault(item => item.Item.Option == detailViewModel?.AssociatedMenuOption);
            });

            #endregion
        }

        /// <summary>
        ///     Menu view model.
        /// </summary>
        public MenuViewModel MenuViewModel { get; set; }

        /// <summary>
        ///     Login view model.
        /// </summary>
        public LoginViewModel LoginViewModel { get; set; }

        /// <summary>
        ///     View model responsible for displaying notifications.
        /// </summary>
        public NotificationViewModel NotificationViewModel { get; set; }

        /// <summary>
        ///     String uniquely identifying the current view model.
        /// </summary>
        public string UrlPathSegment => "Shell";

        /// <summary>
        ///     Host screen to display on.
        /// </summary>
        public IScreen HostScreen { get; }
    }
}