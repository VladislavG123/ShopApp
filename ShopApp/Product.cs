using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Cost { get; set; }

        public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();
    }
}
