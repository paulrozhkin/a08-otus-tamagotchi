using System;
using System.Threading.Tasks;
using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Repositories.Specifications;
using Menu.Domain.Models;

namespace Menu.Domain.Services;

public class MenuService : IMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<MenuItem> _menuRepository;

    public MenuService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _menuRepository = _unitOfWork.Repository<MenuItem>();
    }

    public async Task<PagedList<MenuItem>> GetMenuAsync(int pageNumber, int pageSize)
    {
        var pagedSpecification = new PagedSpecification<MenuItem>(pageNumber, pageSize);
        var menu = await _menuRepository.FindAsync(pagedSpecification);
        var totalCount = await _menuRepository.CountAsync();
        return new PagedList<MenuItem>(menu, totalCount, pageNumber, pageSize);
    }

    public Task<MenuItem> GetMenuItemByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
    {
        throw new NotImplementedException();
    }

    public Task<MenuItem> UpdateMenuItem(MenuItem menuItem)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMenuItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}