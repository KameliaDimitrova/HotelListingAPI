using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HotelListingDbContext context;

    public Repository(HotelListingDbContext context)
    {
        this.context = context;
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
