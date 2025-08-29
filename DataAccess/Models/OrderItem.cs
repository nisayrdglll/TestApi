using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class OrderResponseDTO
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long OrderId { get; set; }

    [Required]
    public long ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public int Deleted { get; set; } = 0;

    // Navigation Properties
    [ForeignKey("OrderId")]
    public virtual CreateOrderRequestDTO Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    public virtual OrderListResponseDTO Product { get; set; } = null!;
}
