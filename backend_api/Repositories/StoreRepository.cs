using Microsoft.EntityFrameworkCore;
using backend_api.Data;
using backend_api.Models;
using backend_api.Repositories.Interfaces;

namespace backend_api.Repositories
{
    /// <summary>
    /// Store repository implementation
    /// </summary>
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;

        public StoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Store?> GetByIdAsync(int id)
        {
            return await _context.Stores.FindAsync(id);
        }

        public async Task<Store?> GetByNameAsync(string name)
        {
            return await _context.Stores.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<IEnumerable<Store>> GetAllAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Store> CreateAsync(Store store)
        {
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<Store> UpdateAsync(Store store)
        {
            _context.Stores.Update(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var store = await GetByIdAsync(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.Stores.AnyAsync(s => s.Name == name);
        }

        public async Task<Store?> GetByManagerIdAsync(int managerId)
        {
            // Manager'ın StoreName'ine göre store bul
            var manager = await _context.Users.FindAsync(managerId);
            if (manager == null) return null;
            
            return await _context.Stores.FirstOrDefaultAsync(s => s.Name == manager.StoreName);
        }
    }
}
