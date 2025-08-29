namespace DataAccess.Schemas
{
    public class MetaSchema
    {
        public int StatusCode { get; set; } = 200;
        public string Status { get; set; } = "OK";
        public bool Successful { get; set; } = true;
        public int ErrorCount { get; set; } = 0;
        public int Page { get; set; } = 0;
        public int Limit { get; set; } = 0;
        public string OrderBy { get; set; } = string.Empty;
        public string SearchText { get; set; } = string.Empty;
        public int PageCount { get; set; } = 0;
        public int Count { get; set; } = 0;
        public List<FilterSchema>? Filters { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}