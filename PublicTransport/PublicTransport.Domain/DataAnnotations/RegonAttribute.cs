using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PublicTransport.Domain.DataAnnotations
{
    public class RegonAttribute : ValidationAttribute
    {
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

        private static bool IsFourteenDigit(IReadOnlyCollection<int> regonDigits)
        {
            if (regonDigits.Count != 14) return false;
            var weights = new List<int> {2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8};
            var controlSum = weights.Zip(regonDigits, (digit, weight) => digit*weight).Sum() % 11 % 10;
            return controlSum == regonDigits.Last();
        }

        private static bool IsNineDigit(IReadOnlyCollection<int> regonDigits)
        {
            if (regonDigits.Count != 9) return false;
            var weights = new List<int> {8, 9, 2, 3, 4, 5, 6, 7};
            var controlSum = weights.Zip(regonDigits, (digit, weight) => digit*weight).Sum() % 11 % 10;
            return controlSum == regonDigits.Last();
        }
    }
}