namespace CafeDomain.Model
{
    public abstract class Entity
    {
        public int Id { get; set; }
    }

    public partial class User : Entity
    {
        public string email { get; set; }
        public string password { get; set; }

        public string name { get; set; }
        public string telephone { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public partial class Category : Entity
    {
        public string category_name { get; set; }
    }

    public partial class OrdersStatus : Entity
    {
        public string status { get; set; } = null!;
    }

    public partial class Product : Entity
    {
        public string photo { get; set; } = null!;
        public string description { get; set; } = null!;

        public Boolean is_avaliable { get; set; }
        public decimal price { get; set; } 
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public partial class Order: Entity
    {
        public virtual ICollection<OrdersStatus> status_id { get; set; } = new HashSet<OrdersStatus>();
        public virtual ICollection<User> user_id { get; set; } = new HashSet<User>();
        public decimal price { get; set; }
        public string adress { get; set; }
        public string payment { get; set; } = null!;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }

    public partial class OrderItem: Entity
    {
        public virtual ICollection<Order> order_id { get; set; }= new HashSet<Order>();
        public virtual ICollection<Product> product_id { get; set; } = new HashSet<Product>();
        public decimal price_for_now { get; set; }
        public int amount { get; set; }
    }

    public partial class ProductCategory: Entity
    {
        public virtual ICollection<Category> category_id { get; set; } = new List<Category>();
        public virtual ICollection<Product> product_id { get; set; } = new List<Product>();
    }

    public partial class OrdersHistory : Entity
    {
        //public virtual ICollection<Order> order_id { get; set; } = new HashSet<Order>();
        public DateTime changed_at { get; set; }
        public int old_status_id { get; set; } 
        public int new_status_id { get; set; }
    }
}
