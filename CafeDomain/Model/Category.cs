using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeDomain.Model;

public partial class Category: Entity
{
  //  public int Id { get; set; }

    [Display(Name = "Категорія")]

    [Required( ErrorMessage = "Назва категорії є обов'язковою")]
    [StringLength(30, MinimumLength = 2) ]
    public string CategoryName { get; set; } = null!;

    [NotMapped]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

}
