namespace DataAccess.Schemas
{
    public class UpdateRequestSchema
    {
        public long Id { get; set; } = 0;
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}