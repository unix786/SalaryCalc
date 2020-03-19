namespace SalaryApp.ViewModels
{
    /// <summary>
    /// Describes a possible value in a "select" type field.
    /// http://w2ui.com/web/docs/1.5/form/fields-enum
    /// </summary>
    public class FieldItem
    {
        public object id { get; set; }
        public string text { get; set; }
    }
}
