using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp
{
    public class Basket : Entity
    {
        public DateTime? PaymentDate { get; set; }
        public bool IsPaid { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
