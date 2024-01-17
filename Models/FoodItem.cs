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
}

