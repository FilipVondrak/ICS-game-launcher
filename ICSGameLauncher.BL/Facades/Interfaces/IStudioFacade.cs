using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.BL.Facades.Interfaces;

public interface IStudioFacade
{
    Task<List<StudioDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StudioDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StudioDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<StudioDto>> GetStudiosWithTitlesAsync(CancellationToken cancellationToken = default);
    Task<StudioDto> InsertAsync(StudioDto studioDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(StudioDto studioDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}