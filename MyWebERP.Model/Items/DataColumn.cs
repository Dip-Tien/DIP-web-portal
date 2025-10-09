namespace MyWebERP.Model
{
    public class DataColumn
    {
        public DataColumn(string columnName, string dataType)
        {
            column_name = columnName;
            data_type = dataType;
        }

        public string table_name { get; set; }
        public string column_name { get; set; }
        public string data_type { get; set; }
    }
}
