using HotelListingAPI.Models;
using static Core.Models.IBaseModel;

namespace HotelListingAPI.Core.Contracts;

public interface IRepository<T> where T: class
{
    Task<T> GetAsync(int? id);

    Task<TResult> GetAsync<TResult>(int? id);

    Task<List<T>> GetAllAsync();

    Task<List<TResult>> GetAllAsync<TResult>();

    Task<PagedResponseModel<TResult>> GetAllAsync<TResult>(QueryParametersRequestModel queryParameters);

    Task<T> AddAsync(T entity);

    Task<TResult> AddAsync<TSource, TResult>(TSource source);

    Task DeleteAsync(int id);

    Task UpdateAsync(T entity);

    Task<bool> ExistsAsync(int id);

    Task UpdateAsync<TSource>(int id, TSource source) where TSource : IBaseModel;
}
