using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeDomain.Model;

public partial class User: Entity
{
   // public int Id { get; set; }

    [Required(ErrorMessage = "Email є обов'язковим")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    [StringLength(100, ErrorMessage = "Email не може бути довше 100 символів")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Name є обов'язковим")]
    [StringLength(20, MinimumLength = 2)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Password є обов'язковим")]
    [DataType(DataType.Password)]
    [StringLength(20, MinimumLength = 6)]
    public string? Password { get; set; }

    [Phone(ErrorMessage = "Невірний формат телефону")]
    [StringLength(14)]
    public string? Telephone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
