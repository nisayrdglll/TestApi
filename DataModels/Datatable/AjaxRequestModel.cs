
namespace DataModels
{
    public class AjaxRequestModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<DTColumn> columns { get; set; }
        public DTSearch search { get; set; }
        public List<DTOrder> order { get; set; }
    }
    public class DTColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public DTSearch search { get; set; }
    }

    public class DTSearch
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class DTOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
