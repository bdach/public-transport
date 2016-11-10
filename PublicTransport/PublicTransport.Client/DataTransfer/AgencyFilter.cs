using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.Agency" /> objects.
    /// </summary>
    public class AgencyFilter : ReactiveObject, IAgencyFilter, IReactiveFilter
    {
        /// <summary>
        ///     Agency name filter.
        /// </summary>
        private string _agencyNameFilter = "";

        /// <summary>
        ///     City name filter.
        /// </summary>
        private string _cityNameFilter = "";

        /// <summary>
        ///     Street name filter.
        /// </summary>
        private string _streetNameFilter = "";

        /// <summary>
        ///     Agency name filter.
        /// </summary>
        public string AgencyNameFilter
        {
            get { return _agencyNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _agencyNameFilter, value); }
        }

        /// <summary>
        ///     City name filter.
        /// </summary>
        public string CityNameFilter
        {
            get { return _cityNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _cityNameFilter, value); }
        }

        /// <summary>
        ///     Street name filter.
        /// </summary>
        public string StreetNameFilter
        {
            get { return _streetNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetNameFilter, value); }
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(AgencyNameFilter) ||
            !string.IsNullOrWhiteSpace(CityNameFilter) ||
            !string.IsNullOrWhiteSpace(StreetNameFilter);
    }
}