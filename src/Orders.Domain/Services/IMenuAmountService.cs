using Orders.Domain.Models;

namespace Orders.Domain.Services
{
    public interface IMenuAmountService
    {
        public Task<int> CalculateAmountForMenuPositions(List<MenuPosition> positions);
    }
}
