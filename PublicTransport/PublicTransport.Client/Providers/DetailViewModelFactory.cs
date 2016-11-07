using System;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Providers
{
    /// <summary>
    ///     Factory for detail view models, used as a navigation helper.
    /// </summary>
    public class DetailViewModelFactory
    {
        /// <summary>
        ///     Returns an <see cref="IDetailViewModel" /> corresponding to the supplied <see cref="MenuOption" />.
        /// </summary>
        /// <param name="screen">The screen the view model is to be shown on.</param>
        /// <param name="option">The <see cref="MenuOption" /> that was clicked in the <see cref="ShellViewModel" />.</param>
        /// <returns>An instance of <see cref="IDetailViewModel" /> corresponding to the supplied <see cref="MenuOption" />.</returns>
        public IDetailViewModel GetViewModel(IScreen screen, MenuOption option)
        {
            switch (option)
            {
                case MenuOption.City:
                    return new FilterCityViewModel(screen);
                case MenuOption.Street:
                    return new FilterStreetViewModel(screen);
                case MenuOption.Agency:
                    return new FilterAgencyViewModel(screen);
                case MenuOption.Route:
                    return new FilterRouteViewModel(screen);
                case MenuOption.Trip:
                    return new EditTripViewModel(screen);
                default:
                    throw new InvalidOperationException("Could not locate view model for this option");
            }
        }
    }
}