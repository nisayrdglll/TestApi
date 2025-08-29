using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Schemas
{
    public class DefaultRequestSchema
    {
        public int Page { get; set; } = 0;
        public int Limit { get; set; } = 0;
        public string SearchText { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public List<FilterSchema>? Filters { get; set; }
        public int SelectedYear { get; set; } = 0;
        public int ProjeOnayDurumSistemKodu { get; set; } = 0;
        public int ProjeDurumSistemKodu { get; set; } = 0;
    }
}