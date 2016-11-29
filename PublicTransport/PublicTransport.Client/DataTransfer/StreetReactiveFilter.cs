using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.Street" /> objects.
    /// </summary>
    public class StreetReactiveFilter : ReactiveObject, IReactiveFilter
    {
        /// <summary>
        ///     City name filter.
        /// </summary>
        private string _cityNameFilter = "";

        /// <summary>
        ///     Street name filter.
        /// </summary>
        private string _streetNameFilter = "";

        /// <summary>
        ///     Street name filter.
        /// </summary>
        public string StreetNameFilter
        {
            get { return _streetNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetNameFilter, value); }
        }

        /// <summary>
        ///     City name filter.
        /// </summary>
        public string CityNameFilter
        {
            get { return _cityNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _cityNameFilter, value); }
        }

        public StreetFilter Convert()
        {
            return new StreetFilter
            {
                CityNameFilter = CityNameFilter,
                StreetNameFilter = StreetNameFilter
            };
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(CityNameFilter) ||
            !string.IsNullOrWhiteSpace(StreetNameFilter);
    }
}