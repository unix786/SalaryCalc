using Newtonsoft.Json;
using System.Collections.Generic;

namespace SalaryApp.ViewModels
{
    /// <summary>
    /// Response to a request from the grid (JavaScript w2grid).
    /// </summary>
    public class GridResponse
    {
        public string status { get; private set; } = "success";
        /// <summary>
        /// http://w2ui.com/web/docs/1.5/w2grid.total
        /// "If your data source is remote the data source should return you total number of records and then this property is ReadOnly."
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// Can be a list of dictionaries with key-value pairs, where the key is field name. Or can be a list of some object.
        /// </summary>
        public IEnumerable<object> records { get; set; }
        public string message { get; set; }

        public static GridResponse Error(string msg) => new GridResponse { status = "error", message = msg };
        public static GridResponse Success => new GridResponse();

        /// <summary>Use this method to prevent .NET from altering dictionary keys by enforcing lowercase.</summary>
        public string Serialize() => JsonConvert.SerializeObject(this);
    }
}
