using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantWebApp.wwwroot.Models
{
    public class FoodItems
    {
        [Key]
        public int ID { get;set; }
        [StringLength(30)]
        public string Item_name { get; set; }
        [StringLength(225)]
        public string Item_desc { get; set; }
        public Nullable<bool> Available { get; set; }
        public bool? Vegetarian { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "Money")]
        public Nullable<decimal> Price { get; set; }
    }
}
