using backend_api.Models;

namespace backend_api.Repositories.Interfaces
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Kullanıcıyı username ile bulur
        /// </summary>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Kullanıcıyı email ile bulur
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Kullanıcıyı ID ile bulur
        /// </summary>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Tüm çalışanları getirir
        /// </summary>
        Task<IEnumerable<User>> GetEmployeesAsync();

        /// <summary>
        /// Belirli bir manager'ın çalışanlarını getirir
        /// </summary>
        Task<IEnumerable<User>> GetEmployeesByManagerAsync(string storeName);

        /// <summary>
        /// Kullanıcı ekler
        /// </summary>
        Task<User> AddAsync(User user);

        /// <summary>
        /// Kullanıcı günceller
        /// </summary>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// Kullanıcı siler
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Username'in benzersiz olup olmadığını kontrol eder
        /// </summary>
        Task<bool> IsUsernameUniqueAsync(string username);

        /// <summary>
        /// Email'in benzersiz olup olmadığını kontrol eder
        /// </summary>
        Task<bool> IsEmailUniqueAsync(string email);

        /// <summary>
        /// Store name'in benzersiz olup olmadığını kontrol eder
        /// </summary>
        Task<bool> IsStoreNameUniqueAsync(string storeName);
    }
}
