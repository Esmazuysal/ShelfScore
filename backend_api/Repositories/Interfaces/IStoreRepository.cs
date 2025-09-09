using backend_api.Models;

namespace backend_api.Repositories.Interfaces
{
    /// <summary>
    /// Store repository interface - Market veri erişim işlemleri
    /// </summary>
    public interface IStoreRepository
    {
        // CRUD Operations
        Task<Store?> GetByIdAsync(int id);
        Task<Store?> GetByNameAsync(string name);
        Task<IEnumerable<Store>> GetAllAsync();
        Task<Store> CreateAsync(Store store);
        Task<Store> UpdateAsync(Store store);
        Task<bool> DeleteAsync(int id);
        
        // Business Logic Methods
        Task<bool> ExistsAsync(string name);
        Task<Store?> GetByManagerIdAsync(int managerId);
    }
}
