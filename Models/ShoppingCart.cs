using EShopp.Models.EF;

namespace Eshopp.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }

        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
        }
        public void AddToCart(ShoppingCartItem item, int quantity)
        {
            var existingItem = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Price * existingItem.Quantity;
            }
            else
            {
                item.Quantity = quantity;
                item.TotalPrice = item.Price * quantity;
                Items.Add(item);
            }
        }

        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }
        }


        public void UpdateQuantity(int id, int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);

            if (checkExits != null)
            {
                if (quantity <= 0) // Ngăn số lượng âm hoặc 0
                {
                    Items.Remove(checkExits);
                }
                else
                {
                    checkExits.Quantity = quantity;
                    checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
                }
            }
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TotalPrice);

        }
        public decimal GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);

        }
        public void ClearCart()
        {
            Items.Clear();
        }

        public class ShoppingCartItem
        {
            public int ProductId { get; set; }
            public string? ProductName { get; set; }
            public string? Alias {  get; set; }
            public string? CategoryName {  get; set; }
            public string? ProductImage { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}

