using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PD_Store.ViewModels.Product
{
    public class ProductVM
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }

        public double Price { get; set; }

        public double? Weight { get; set; }

        public double? NRC { get; set; } //Chỉ số hấp thụ âm thanh

        public double? STC { get; set; } //Chỉ số cách âm

        public string? FireResistance { get; set; } //Mức độ chống cháy (Trung bình, Cao, Không chống cháy)

        public int StockQuantity { get; set; }

        public string? Desc { get; set; }

        public List<ProductImagesVM>? ListProductImages { get; set; }
    }

    public class ProductImagesVM
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }
        public virtual ProductVM? Product { get; set; }

        public required string ImageLink { get; set; }
    }

    public class ApplicationAreasVM
    {
        public int Id { get; set; }

        public required string AreaName { get; set; }

        public string? Note { get; set; }
    }
}