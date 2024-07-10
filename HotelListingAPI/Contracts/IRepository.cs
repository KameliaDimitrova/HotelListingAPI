using HotelListingAPI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelListingAPI.Contracts;

public interface IRepository<T> where T: class
{
    Task<T?> GetAsync(int id);

    Task<List<T>> GetAllAsync();

    Task<PagedResponseModel<TResult>> GetAllAsync<TResult>(QueryParametersRequestModel request);

    Task<T> AddAsync(T  entity);

    Task DeleteAsync(int id);

    Task UpdateAsync(T entity);

    Task<bool> ExistsAsync(int id);
}
