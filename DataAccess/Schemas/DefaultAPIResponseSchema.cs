using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Schemas
{
    public class DefaultAPIResponseSchema
    {
        public APIMetaInformationSchema? Meta { get; set; }
        public IList? Items { get; set; }
        public List<ErrorSchema>? Errors { get; set; }
    }
}