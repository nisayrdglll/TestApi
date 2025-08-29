namespace DataAccess.Schemas
{
    public class FormMetaInformationSchema
    {
        public int StatusCode { get; set; } = 200;
        public int MethodType { get; set; } = 0;
        public string Status { get; set; } = "OK";
        public bool Successful { get; set; } = true;
        public int MessageCount { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;

    }
}