using System.Collections.Generic;

namespace SalaryApp.ViewModels
{
    /// <summary>
    /// Options for w2field (JavaScript).
    /// <para/>http://w2ui.com/web/docs/1.3/form/fields
    /// </summary>
    public class FieldOptions
    {
        /// <summary>
        /// Some of the possible values: text, int, float, date, time, datetime, checkbox, select/enum?
        /// </summary>
        public string type;
        /// <summary>
        /// List of items for type "select".
        /// </summary>
        public IEnumerable<FieldItem> items;
    }
}
