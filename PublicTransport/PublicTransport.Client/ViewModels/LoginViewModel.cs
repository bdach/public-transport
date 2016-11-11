using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels
{
    public class LoginViewModel : ReactiveObject, IRoutableViewModel
    {
        /// <summary>
        ///     Service used for logging into the application.
        /// </summary>
        private ILoginService _loginService;

        /// <summary>
        ///     Password box storing the password entered by the user.
        /// </summary>
        private PasswordBox _passwordBox;

        /// <summary>
        ///     Object containing information concerning the currently logged in user.
        /// </summary>
        private UserInfo _userInfo;

        /// <summary>
        ///     Stores the username entered by the user.
        /// </summary>
        private string _username = "";

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display the view model on.</param>
        /// <param name="loginService">Login service to inject.</param>
        public LoginViewModel(IScreen screen, ILoginService loginService = null)
        {
            HostScreen = screen;
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();

            #region Send login request

            var canSendRequest = this.WhenAnyValue(v => v.Username)
                .Select(s => !string.IsNullOrWhiteSpace(s));
            SendLoginRequest = ReactiveCommand.CreateAsyncTask(canSendRequest, async _ =>
            {
                var loginData =
                    await Task.Run(() => _loginService.RequestLogin(new LoginData(Username, PasswordBox?.Password)));
                Username = null;
                if (PasswordBox != null) PasswordBox.Password = null;
                return loginData;
            });
            SendLoginRequest.Subscribe(s =>
            {
                UserInfo = s;
                _loginService.Dispose();
                _loginService = new LoginService();
            });
            SendLoginRequest.ThrownExceptions
                .SelectMany(ex => UserError.Throw("Invalid username or password.", ex))
                .Subscribe();

            #endregion
        }

        /// <summary>
        ///     Command responsible for sending the login request.
        /// </summary>
        public ReactiveCommand<UserInfo> SendLoginRequest { get; }

        /// <summary>
        ///     Stores the username entered by the user.
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { this.RaiseAndSetIfChanged(ref _username, value); }
        }

        /// <summary>
        ///     Password box storing the password entered by the user.
        /// </summary>
        public PasswordBox PasswordBox
        {
            get { return _passwordBox; }
            set { this.RaiseAndSetIfChanged(ref _passwordBox, value); }
        }

        /// <summary>
        ///     Object containing information concerning the currently logged in user.
        /// </summary>
        public UserInfo UserInfo
        {
            get { return _userInfo; }
            set { this.RaiseAndSetIfChanged(ref _userInfo, value); }
        }

        /// <summary>
        ///     String uniquely identifying the current view model.
        /// </summary>
        public string UrlPathSegment => "Login";

        /// <summary>
        ///     Host screen to display on.
        /// </summary>
        public IScreen HostScreen { get; }
    }
}