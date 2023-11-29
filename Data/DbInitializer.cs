using RestaurantWebApp.Models;

namespace RestaurantWebApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(RestaurantWebAppContext context)
        {
            // Look for any food itmes.
            if (context.FoodItems.Any())
            {
                return;   // DB has been seeded
            }

            var fooditems = new FoodItem[]
            {
                new FoodItem{Item_name="Pani Puri",Item_desc="Also known as Gol Gappa, Pani Puri is a flavor bomb of spice, sweet and savory chutneys as well as Mouthwatering potato fillings",Available=true,Vegetarian=true},
                new FoodItem{Item_name="Bombay Sandwich",Item_desc="Seasoned potatoes, tomatoes, onions, house chutneys and a whole bunch of cheese for an Indian grilled cheese",Available=true,Vegetarian=true},
                new FoodItem{Item_name="Masala Dosa",Item_desc="A Classic South Indian meal, served with Sambar, Coconut chutney, and filled with masala potatos",Available=true,Vegetarian=true},
                new FoodItem{Item_name="Ponk Bhel",Item_desc="Ponk, also known as sorghum grain, is cookies and then topped with lemon, cilantro and green chutney",Available=true,Vegetarian=true},
                new FoodItem{Item_name="Patra",Item_desc="A Gujurati Specialty, made by rolling taro leaves and gram flour into pinwheels and then frying them",Available=true,Vegetarian=true},
                new FoodItem{Item_name="Vada Pav",Item_desc="Made by smashing masala potatoes, shaping them into balls and frying as an Indian burger patty",Available=true,Vegetarian=true},
                new FoodItem{Item_name="Navratan korma",Item_desc="Navratan meaning 9 gems, this korma has 9 different vegetables, seeds and nuts all in a creamy sauce",Available=true,Vegetarian=true}

           /*     new FoodItem{Item_name="Cottage Pie",Item_desc="Our tasty cottage pie packed full of lean minced beef and an assortment of vegetables",Available=true,Vegetarian=false},
                new FoodItem{Item_name="Haggis,Neeps and Tatties",Item_desc="Scotland national Haggis dish. Sheep’s heart, liver, and lungs are minced, mixed with suet and oatmeal, then seasoned with onion, cayenne, and our secret spice. Served with boiled turnips and potatoes (‘neeps and tatties’)",Available=true,Vegetarian=false},
                new FoodItem{Item_name="Bangers and Mash",Item_desc="Succulent sausages nestled on a bed of buttery mashed potatoes and drenched in a rich onion gravy",Available=true,Vegetarian=false},
                new FoodItem{Item_name="Toad in the Hole",Item_desc="Ultimate toad-in-the-hole with caramelised onion gravy",Available=true,Vegetarian=false}*/
            };

            context.FoodItems.AddRange(fooditems);
            context.SaveChanges();

        }
    }
}
