using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    /// <summary>
    ///     View model responsible for notifying the user about errors.
    /// </summary>
    public class NotificationViewModel : ReactiveObject
    {
        /// <summary>
        ///     Contains information whether tne notification view model is currently visible.
        /// </summary>
        private bool _isVisible;

        /// <summary>
        ///     The message to be displayed to the user.
        /// </summary>
        private string _message;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public NotificationViewModel()
        {
            #region Ten-second notification hang time

            this.WhenAnyValue(vm => vm.IsVisible)
                .Where(b => b)
                .Throttle(TimeSpan.FromSeconds(10), RxApp.MainThreadScheduler)
                .Subscribe(_ => IsVisible = false);

            #endregion

            #region Close command for manually closing the notification

            Close = ReactiveCommand.Create();
            Close.Subscribe(_ => IsVisible = false);

            #endregion
        }

        /// <summary>
        ///     Contains information whether tne notification view model is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { this.RaiseAndSetIfChanged(ref _isVisible, value); }
        }

        /// <summary>
        ///     The message to be displayed to the user.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { this.RaiseAndSetIfChanged(ref _message, value); }
        }

        /// <summary>
        ///     Command used to manually close the notification bar.
        /// </summary>
        public ReactiveCommand<object> Close { get; protected set; }
    }
}