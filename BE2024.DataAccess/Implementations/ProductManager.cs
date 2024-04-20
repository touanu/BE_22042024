﻿using BE2024.DataAccess.Layers;
using BE2024.DataAccess.Objects;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BE2024.DataAccess.Implementations
{
    public class ProductManager : IProductManager
    {
        private readonly List<Product> products = new List<Product>()
        {
            new Product("P001", "Sữa ông thọ", 18000, 100),
            new Product("P002", "Tai nghe Bluetooth", 200000, 50),
            new Product("P003", "Sách Đắc nhân tâm", 57000, 200),
            new Product("P004", "Hộp khẩu trang", 25000, 440),
            new Product("P005", "Cân điện tử", 216000, 190)
        };
        private readonly List<Product> buyList = new List<Product>();
        private int BuyQuantity { get; set; }
        private double TotalBill = 0;
        public ReturnData BuyProduct(Product product)
        {
            ReturnData returnData = new ReturnData();
            
            if (product == null)
            {
                returnData.ErrorCode = -1;
                returnData.Message = "Sản phẩm không được để trống!";
                return returnData;
            }

            int index = products.IndexOf(product);
            if (index == -1)
            {
                returnData.ErrorCode = -2;
                returnData.Message = "Sản phẩm này không tồn tại!";
                return returnData;
            }

            if (product.Quantity <= 0 || product.Quantity > products[index].Quantity)
            {
                returnData.ErrorCode = -3;
                returnData.Message = "Số lượng không hợp lệ!";
                return returnData;
            }

            buyList.Add(product);
            BuyQuantity += product.Quantity;

            if (product.Quantity == products[index].Quantity)
                products.Remove(product);
            else
                products[index].Quantity -= product.Quantity;
            
            return returnData;
        }
        public string ShowBuyProducts()
        {
            string footer;

            if (BuyQuantity > 5)
                footer = $"\nThành tiền: {TotalBill * 5} | Chiết khấu: 5%";
            else
                footer = $"\nThành tiền: {TotalBill} | Chiết khấu: 0%";

            return ShowProduct(buyList) + footer;
        }
        public string ShowAvailableProducts()
        {
            return ShowProduct(products);
        }
        private string ShowProduct(List<Product> products)
        {
            CultureInfo provider = new CultureInfo("vi-VN");
            StringBuilder builder = new StringBuilder();
            string format = "{0, -10}{1,-20}{2,-16:C0}{3,-11}{4,-16:C0}";

            string header = string.Format(provider, format,
                    "Mã SP",
                    "Tên",
                    "Giá",
                    "Số lượng",
                    "Đơn giá");
            builder.AppendLine(header);

            foreach (Product product in products)
            {
                string content = string.Format(provider, format,
                    product.ID,
                    product.Name,
                    product.TotalPrice,
                    product.Quantity,
                    product.Price);
                TotalBill += product.TotalPrice;
                builder.AppendLine(content);
            }

            return builder.ToString();
        }
    }
}
