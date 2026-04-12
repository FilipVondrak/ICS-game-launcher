using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.BL.Facades.Interfaces;

public interface ICategoryFacade
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<CategoryDto>> GetCategoriesWithTitlesAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto> InsertAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}