namespace PublicTransport.Client.Models
{
    /// <summary>
    ///     Class representing a single menu item in the master view (on the left side of the window)
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="label">Label to be used on the menu item.</param>
        /// <param name="option"><see cref="MenuOption" /> enum value uniquely identifying the menu item.</param>
        public MenuItem(string label, MenuOption option)
        {
            Label = label;
            Option = option;
        }

        /// <summary>
        ///     <see cref="MenuOption" /> enum value uniquely identifying the menu item.
        /// </summary>
        public MenuOption Option { get; set; }

        /// <summary>
        ///     The menu item label. This text appears to the user on the UI.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     Returns the string representation of this item.
        /// </summary>
        /// <returns>Menu item label.</returns>
        public override string ToString()
        {
            return Label;
        }
    }

    /// <summary>
    ///     Enumeration used for identifying and matching view models to menu items.
    /// </summary>
    public enum MenuOption
    {
        /// <summary>
        ///     Represents the <see cref="ViewModels.Filter.FilterCityViewModel" /> view model.
        /// </summary>
        City,

        /// <summary>
        ///     Represents the <see cref="ViewModels.Filter.FilterStreetViewModel" /> view model.
        /// </summary>
        Street,

        /// <summary>
        ///     Represents the <see cref="ViewModels.Filter.FilterAgencyViewModel" /> view model.
        /// </summary>
        Agency,

        /// <summary>
        ///     Represents the <see cref="ViewModels.Filter.FilterRouteViewModel" /> view model.
        /// </summary>
        Agency,

        /// <summary>
        ///     Represents the <see cref="ViewModels.Edit.EditZoneViewModel" /> view model.
        /// </summary>
        Zone,

        /// <summary>
        ///     Represents the <see cref="ViewModels.Edit.EditStopViewModel" /> view model.
        /// </summary>
        Stop,

        /// <summary>
        ///     Represents the <see cref="ViewModels.Edit.EditFareViewModel" /> view model.
        /// </summary>
        Fare,


        /// <summary>
        ///     Represents the <see cref="ViewModels.Edit.EditRouteViewModel" /> view model.
        /// </summary>
        Route,


        /// <summary>
        ///     Represents the <see cref="ViewModels.Edit.EditTripViewModel" /> view model.
        /// </summary>
        Trip
    }
}