namespace DataAccess.Schemas
{
    public class APIUpdateMetaInformationSchema
    {
        public int StatusCode { get; set; } = 200;
        public string Status { get; set; } = "OK";
        public bool Successful { get; set; } = true;
        public int ErrorCount { get; set; } = 0;
        public List<ErrorSchema>? Errors { get; set; }
        public List<UpdateRequestSchema>? Updates { get; set; }
    }
}