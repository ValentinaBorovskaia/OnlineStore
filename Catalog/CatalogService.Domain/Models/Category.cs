using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Domain.Models;

public partial class Category
{
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual Category? Parent { get; set; }
}
