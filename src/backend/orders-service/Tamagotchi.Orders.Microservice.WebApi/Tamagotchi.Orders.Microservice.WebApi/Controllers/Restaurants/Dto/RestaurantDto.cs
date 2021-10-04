namespace Tamagotchi.Orders.Microservice.WebApi.Controllers.Restaurants.Dto
{
    public class RestaurantDto
    {
        /// <summary>
        /// Restaurant identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of restaurant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full address
        /// </summary>
        public string Address { get; set; } 
    }
}