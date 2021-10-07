namespace OrderQueue.API.Models.Dish
{
    public class DishResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DishStatusResponse Status { get; set; }
    }
}
