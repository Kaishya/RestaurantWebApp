using System.ComponentModel.DataAnnotations;

namespace RestaurantWebApp.Data
{
	public class OrderItem
	{
		[Required]
		public int OrderNo { get; set; }
		[Required]
		public int StockID { get; set; }
		[Required]
		public int Quantity { get; set; }
	}
}
