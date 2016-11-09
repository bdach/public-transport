using System.Windows;
using PublicTransport.Client.Providers;
using PublicTransport.Client.ViewModels;
using PublicTransport.Client.Views;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static AppBootstrapper Bootstrapper;
        public static ShellView ShellView;

        public App()
        {
            Bootstrapper = new AppBootstrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // This is added to fix problems with ReactiveList.AddRange (multiple notifications firing)
            RxApp.SupportsRangeNotifications = false;
            ShellView = (ShellView)Locator.Current.GetService<IViewFor<ShellViewModel>>();
            ShellView.Show();
        }
    }
}
