using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    /// <summary>
    ///     Placeholder view model. Displayed in the detail portion of the window when no menu item is displayed (or the user
    ///     has closed all the views)
    /// </summary>
    public class PlaceholderViewModel : ReactiveObject, IRoutableViewModel
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Host screen to display on.</param>
        public PlaceholderViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        /// <summary>
        ///     String uniquely identifying the current view model.
        /// </summary>
        public string UrlPathSegment => "Placeholder";

        /// <summary>
        ///     Host screen to display on.
        /// </summary>
        public IScreen HostScreen { get; }
    }
}