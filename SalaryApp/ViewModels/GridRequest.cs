using Newtonsoft.Json;
using System.Collections.Generic;

namespace SalaryApp.ViewModels
{
    /// <summary>
    /// An Ajax request from w2grid.
    /// </summary>
    public class GridRequest
    {
        public static GridRequest Deserialize(string request)
            => JsonConvert.DeserializeObject<GridRequest>(
                request,
                new JsonSerializerSettings { FloatParseHandling = FloatParseHandling.Decimal }
                );

        public int Offset { get; set; }
        public int Limit { get; set; }

        public List<Dictionary<string, object>> Changes;
        [JsonProperty(PropertyName = "recid")]
        public List<int> Selected { get; set; }

        public object Search { get; set; }
        public object Sort { get; set; }
    }
}
