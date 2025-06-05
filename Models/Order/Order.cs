using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PD_Store.Models.Auth;
using PD_Store.Models.Product;

namespace PD_Store.Models.Order
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public required string AddressReceiver { get; set; } //Địa chỉ giao hàng

        public required string PhoneNumberReceiver { get; set; } //SDT người nhận

        public required string NameReceiver { get; set; } // Tên người nhận

        public string? Status { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdOrder { get; set; }
        [ForeignKey(nameof(IdOrder))]
        public virtual Order? Order { get; set; }

        public int IdProduct { get; set; }
        [ForeignKey(nameof(IdProduct))]
        public virtual Products? Product { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public int? IdSale { get; set; }
        [ForeignKey(nameof(IdSale))]
        public virtual Sales? Sales { get; set; }
    }

    public class Sales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string NameSales { get; set; }

        public string? Desc { get; set; }

        public double Factor { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int QuantitySales { get; set; }

    }

}