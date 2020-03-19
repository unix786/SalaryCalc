namespace SalaryApp.ViewModels
{
    /// <summary>
    /// http://w2ui.com/web/docs/1.5/w2grid.columns
    /// <para/>Must specify <see cref="size"/>.
    /// </summary>
    public sealed class GridColumn
    {
        public string field;
        private string _text;
        public string text
        {
            get => _text ?? field;
            set => _text = value;
        }
        //public string caption => text;
        /// <summary>
        /// Can be either "px" or "%".
        /// </summary>
        public string size;
        public bool hidden;
        public bool sortable;
        /// <summary>
        /// Format string. Can also specify a JS function, but can only do it from the view.
        /// </summary>
        /// <remarks>
        /// From documentation:
        /// There are several predefined formatters. They are (for number and float, XX - defines number of digits after dot):
        /// int
        /// float:XX
        /// number:XX
        /// money
        /// percent
        /// age
        /// date
        /// </remarks>
        public string render;
        public FieldOptions editable;
        /// <summary>
        /// http://w2ui.com/web/docs/1.5/w2grid.searches
        /// </summary>
        /// <remarks>
        /// http://w2ui.com/web/docs/1.5/w2grid.columns
        /// <para/>Здесь пишет, что по умолчанию false, но можно указать строковое значение, указывающее тип поля.
        /// Кроме того, можно указать объект со свойствами объекта "searches", но конструктор w2grid указывает код и название поля за нас.
        /// <para/>Про w2field: http://w2ui.com/web/docs/1.5/w2form.fields
        /// </remarks>
        public object searchable;
    }
}
