using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTOs
{
    public class CreateOrderRequestDTO
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ShippingAddress { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "En az 1 ürün seçilmelidir.")]
        public List<CreateOrderItemDTO> OrderItems { get; set; } = new List<CreateOrderItemDTO>();
    }

    public class CreateOrderItemDTO
    {
        [Required]
        public long ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den büyük olmalıdır.")]
        public int Quantity { get; set; }
    }
}