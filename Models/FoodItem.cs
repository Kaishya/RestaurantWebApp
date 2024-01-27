using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantWebApp.Models
{
    public class FoodItem
    {
        [Key]
        public int ID { get; set; }
        [StringLength(30)]
        public string Item_name { get; set; }
        [StringLength(225)]
        public string Item_desc { get; set; }
        public Nullable<bool> Available { get; set; }
        public bool? Vegetarian { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "Money")]
        public Nullable<decimal> Price { get; set; }
        public string ImageDescription { get; set; }
        public byte[] ImageData { get; set; }
    }

    public class CheckoutCustomer
    {
        [Key]
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int BasketID { get; set; }
    }

    public class Basket
    {
        [Key]
        public int BasketID { get; set; }
    }

    public class BasketItem
    {
        [Required]
        public int StockID { get; set; }
        [Required]
        public int BasketID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

}

