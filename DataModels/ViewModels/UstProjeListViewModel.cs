
namespace DataModels.ViewModels;

public partial class UstProjeListViewModel
{
	public string Id { get; set; } = null!;

	public string? AnaustprojeId { get; set; }

	public int? TanimlamaDurumId { get; set; }

	public short? ProjeTeklifiVerilebilir { get; set; }

	public string? FaaliyetaltfaaliyetId { get; set; }

	public short? Altfaaliyetvar { get; set; }

	public string? Kodu { get; set; }

	public string? Adi { get; set; }

	public string? KisaAdi { get; set; }

	public string? OrganizasyonBirimiIdListesi { get; set; }

	public string? CreatedBy { get; set; }

	public DateTime? CreatedAt { get; set; }

	public string? UpdatedBy { get; set; }

	public DateTime? UpdatedAt { get; set; }

	public string? DeletedBy { get; set; }

	public DateTime? DeletedAt { get; set; }

	public short? Deleted { get; set; }
	public string? CreationDateDisplayText { get; set; }
	public string? UpdatedAtDisplayText { get; set; }
	public string? CreatedByDisplayText { get; set; }
	public string? UpdatedByDisplayText { get; set; }
	public string? TanimlamaDurumDisplayName { get; set; }
	public string? TanimlamaDurumDisplayColor { get; set; }
	public string? AnaUstProjeDisplayName { get; set; }
	public string? FaaliyetAltFaaliyetDisplayName { get; set; }
    public string FaaliyetDisplayName { get; set; }
    public string AltFaaliyetDisplayName { get; set; }
    public string? FaaliyetAltFaaliyetDisplayColor { get; set; }
	public string? AltfaaliyetvarDisplayText { get; set; }
	public string? AltfaaliyetvarDisplayCSSClass { get; set; }
	public string? ProjeTeklifiVerilebilirDisplayText { get; set; }
	public string? ProjeTeklifiVerilebilirDisplayCSSClass { get; set; }
	public string? OrganizasyonBirimiListesiDisplayName { get; set; }
}
