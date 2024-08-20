namespace UsersAPI.Repositories
{
    /// <summary>
    /// 제네릭 리포지토리 인터페이스
    /// </summary>
    /// <typeparam name="T">베이스 모델</typeparam>
    /// <typeparam name="K">업데이트 모델</typeparam>
    public interface IGenericRepository<T, K> where T : class where K : class
    {
        Task<T?> GetByIdAsync(int id);
        //Task<IReadOnlyList<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<int> UpdateByIdAsync(K entity);
        Task<int> DeleteByIdAsync(int id);
    }
}
