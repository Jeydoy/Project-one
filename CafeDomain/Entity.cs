using System.ComponentModel.DataAnnotations;

namespace CafeDomain.Model
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }

   
}
