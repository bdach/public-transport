using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.Street" /> objects.
    /// </summary>
    public class StreetFilter : ReactiveObject, IStreetFilter
    {
        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        private readonly ObservableAsPropertyHelper<bool> _isValid;

        /// <summary>
        ///     City name filter.
        /// </summary>
        private string _cityNameFilter = "";

        /// <summary>
        ///     Street name filter.
        /// </summary>
        private string _streetNameFilter = "";

        /// <summary>
        ///     Constructor.
        /// </summary>
        public StreetFilter()
        {
            _isValid = this.WhenAnyValue(x => x.StreetNameFilter, x => x.CityNameFilter,
                    (s, c) => !string.IsNullOrWhiteSpace(s) || !string.IsNullOrWhiteSpace(c))
                .ToProperty(this, x => x.IsValid);
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid => _isValid.Value;

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
    }
}