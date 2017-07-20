using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {

        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Product product,int quatity)
        {
            CartLine line = lineCollection.Where(p => p.Product.ProductID == product.ProductID).FirstOrDefault();

            if(line==null)
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quatity });

            }
            else
            {
                line.Quantity += quatity;
            }
        }


        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        public decimal ComputerTotalValue()
        {
            return lineCollection.Sum(e => e.Product.price * e.Quantity);
        }

        public  void Clear()
        {
            lineCollection.Clear();

        }

        public IEnumerable<CartLine> Lines
        {
            get
            {
                return lineCollection;
            }
        }
    }


    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
