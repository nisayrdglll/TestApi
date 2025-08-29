
namespace DataModels
{
    public class DataResponseModel<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IList<T>data { get; set; }
    }
}
