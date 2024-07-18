using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListingAPI.Core.Contracts;
using HotelListingAPI.Infrastructure;
using HotelListingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Core.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HotelListingDbContext context;
    private readonly IMapper mapper;

    public Repository(
        HotelListingDbContext context,
        IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public async Task<T> AddAsync(T entity)
    {
        await this.context.AddAsync(entity);
        await this.context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await this.context.Set<T>().FindAsync(id);
        if(entity is null)
        {
            return;
        }
        this.context.Set<T>().Remove(entity);
        await this.context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var entity = await this.context.Set<T>().FindAsync(id);

        return entity != null;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await this.context.Set<T>().ToListAsync();
    }

    public async Task<PagedResponseModel<TResult>> GetAllAsync<TResult>(QueryParametersRequestModel request)
    {
        var totalSize = await this.context.Set<T>().CountAsync();
        var items = await this.context.Set<T>()
            .Skip(request.StartIngex)
            .Take(request.PageSize)
            .ProjectTo<TResult>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResponseModel<TResult>
        {
            Items = items,
            PageNumber = request.StartIngex,
            RecordNumber = request.PageSize,
            TotalCount = totalSize
        };
    }

    public async Task<T?> GetAsync(int id)
    {
        return await this.context.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        this.context.Update(entity);
        await this.context.SaveChangesAsync();
    }
}
