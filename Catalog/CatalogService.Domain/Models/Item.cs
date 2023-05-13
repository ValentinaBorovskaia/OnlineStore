using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Domain.Models;

public partial class Item
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int CategoryId { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Amount { get; set; }

    public virtual Category Category { get; set; } = null!;
}
