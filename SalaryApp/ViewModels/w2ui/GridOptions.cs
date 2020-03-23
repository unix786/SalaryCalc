using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SalaryApp.ViewModels
{
    /// <summary>
    /// http://w2ui.com/web/docs/1.5/grid/properties
    /// </summary>
    public sealed class GridOptions
    {
        public IEnumerable<GridColumn> Columns { get; set; } = Array.Empty<GridColumn>();

        /// <summary>
        /// By default the grid uses this field as an identifier.
        /// </summary>
        public const string DefaultIDField = "recid";

        /// <summary>
        /// http://w2ui.com/web/docs/1.5/w2grid.recid
        /// Does not work properly. Better leave default (<see cref="DefaultIDField"/>).
        /// </summary>
        public string IDField { get; set; } = DefaultIDField;

        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.None,
        };

        public object show { get; set; }

        public string Serialize(IUrlHelper url) => JsonConvert.SerializeObject(
            new
            {
                columns = Columns,
                recid = IDField,
                url = new
                {
                    get = url.Action("GridGet"),
                    save = url.Action("GridSave"),
                    remove = url.Action("GridDelete"),
                },
                show
            },
            serializerSettings);
    }
}
