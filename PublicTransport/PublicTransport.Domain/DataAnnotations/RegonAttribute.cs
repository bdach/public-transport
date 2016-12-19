using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PublicTransport.Domain.DataAnnotations
{
    /// <summary>
    ///     Data annotation facilitating the validation of REGON numbers.
    /// </summary>
    public class RegonAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Checks whether the supplied value is a valid REGON number.
        /// </summary>
        /// <param name="value">Value to check. Should be a string.</param>
        /// <returns>True, if the value is a valid REGON; false otherwise.</returns>
        public override bool IsValid(object value)
        {
            if (!(value is string)) return false;
            var regon = (string) value;
            long unused;
            if (long.TryParse(regon, out unused) == false) return false;
            var regonDigits = regon
                .Select(c => int.Parse(c.ToString()))
                .ToList();
            return IsNineDigit(regonDigits) || IsFourteenDigit(regonDigits);
        }

        /// <summary>
        ///     Checks whether the supplied digits form a valid 14-digit REGON number.
        /// </summary>
        /// <param name="regonDigits">REGON digits to check.</param>
        /// <returns>True, if the value is a valid 14-digit REGON; false otherwise.</returns>
        private static bool IsFourteenDigit(IReadOnlyCollection<int> regonDigits)
        {
            if (regonDigits.Count != 14) return false;
            var weights = new List<int> {2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8};
            var controlSum = weights.Zip(regonDigits, (digit, weight) => digit*weight).Sum()%11%10;
            return controlSum == regonDigits.Last();
        }

        /// <summary>
        ///     Checks whether the supplied digits form a valid 9-digit REGON number.
        /// </summary>
        /// <param name="regonDigits">REGON digits to check.</param>
        /// <returns>True, if the value is a valid 9-digit REGON; false otherwise.</returns>
        private static bool IsNineDigit(IReadOnlyCollection<int> regonDigits)
        {
            if (regonDigits.Count != 9) return false;
            var weights = new List<int> {8, 9, 2, 3, 4, 5, 6, 7};
            var controlSum = weights.Zip(regonDigits, (digit, weight) => digit*weight).Sum()%11%10;
            return controlSum == regonDigits.Last();
        }
    }
}