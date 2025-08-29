using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Schemas
{
    public class DefaultFormResponseSchema
    {
        public FormMetaInformationSchema? Meta { get; set; }
        public List<MessageSchema>? Messages { get; set; }
        public List<ErrorSchema>? Errors { get; set; }
    }
}