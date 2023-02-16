using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ÖFMSluträkningUI.Validation {
    internal static class DataValidation {
        public static bool TryParseDecimal(string value) {

            return Decimal.TryParse(value, out decimal d);
        }

        public static bool TryParseDateTime(string value) {

            return DateTime.TryParse(value, out DateTime d);
        }

        public static bool TryParseInt32(string value) {

            return Int32.TryParse(value, out int d);
        }
    }
}
