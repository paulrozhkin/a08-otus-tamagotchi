namespace OrderQueue.Core.Models
{
    public enum KitchenOrderStatus
    {
        // Не взят
        Wait,
        // Готовится
        Cooking,
        // Готово к подаче
        ReadyToServe,
        // Подано
        Served
    }
}
