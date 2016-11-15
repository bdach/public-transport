using System.Reactive.Linq;
using System.Windows;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    /// Interaction logic for NotificationView.xaml
    /// </summary>
    public partial class NotificationView : IViewFor<NotificationViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(NotificationViewModel), typeof(NotificationView), new PropertyMetadata(default(NotificationViewModel)));

        public NotificationViewModel ViewModel
        {
            get { return (NotificationViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public NotificationView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Message, v => v.MessageTextBlock.Text);
            this.OneWayBind(ViewModel, vm => vm.IsVisible, v => v.Visibility);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            UserError.RegisterHandler(err =>
            {
                ViewModel.Message = err.ErrorMessage;
                ViewModel.IsVisible = true;
                // For now, we'll just cancel all operations and tell the user nothing can be done,
                // since most errors are basically unrecoverable (DB failure). Everything else
                // such as invalid queries should be caught by the UI.
                return Observable.Return(RecoveryOptionResult.CancelOperation);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (NotificationViewModel) value; }
        }
    }
}
