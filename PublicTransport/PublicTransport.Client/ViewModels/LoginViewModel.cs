using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class LoginViewModel : ReactiveObject, IRoutableViewModel
    {
        private LoginService _loginService;
        private UserInfo _userInfo;
        private string _username = "";
        private PasswordBox _passwordBox;

        public LoginViewModel(IScreen screen)
        {
            HostScreen = screen;
            _loginService = new LoginService();

            #region Send login request

            var canSendRequest = this.WhenAnyValue(v => v.Username)
                .Select(s => !string.IsNullOrWhiteSpace(s));
            SendLoginRequest = ReactiveCommand.CreateAsyncTask(canSendRequest, async _ =>
            {
                Debug.WriteLine(PasswordBox.Password);
                var loginData = await Task.Run(() => _loginService.RequestLogin(new LoginData(Username, PasswordBox.Password)));
                Username = null;
                PasswordBox.Password = null;
                return loginData;
            });
            SendLoginRequest.Subscribe(s =>
            {
                UserInfo = s;
                _loginService.Dispose();
                _loginService = new LoginService();
            });
            SendLoginRequest.ThrownExceptions
                .Subscribe(ex => UserError.Throw("Invalid username or password.", ex));

            #endregion
        }

        public ReactiveCommand<UserInfo> SendLoginRequest { get; }

        public string Username
        {
            get { return _username; }
            set { this.RaiseAndSetIfChanged(ref _username, value); }
        }

        public PasswordBox PasswordBox
        {
            get { return _passwordBox; }
            set { this.RaiseAndSetIfChanged(ref _passwordBox, value); }
        }

        public UserInfo UserInfo
        {
            get { return _userInfo; }
            set { this.RaiseAndSetIfChanged(ref _userInfo, value); }
        }

        public string UrlPathSegment => "Login";
        public IScreen HostScreen { get; }
    }
}