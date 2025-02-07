﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Exceptions;
using Core.Models;
using HotelListingAPI.Core.Contracts;
using HotelListingAPI.Exceptions;
using HotelListingAPI.Infrastructure;
using HotelListingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Core.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public Repository(HotelListingDbContext context, IMapper mapper)
    {
        this._context = context;
        this._mapper = mapper;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
    {
        var entity = _mapper.Map<T>(source);

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<TResult>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, id);
        }

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<PagedResponseModel<TResult>> GetAllAsync<TResult>(QueryParametersRequestModel queryParameters)
    {
        var totalSize = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>()
            .Skip(queryParameters.StartIndex)
            .Take(queryParameters.PageSize)
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResponseModel<TResult>
        {
            Items = items,
            PageNumber = queryParameters.StartIndex,
            RecordNumber = queryParameters.PageSize,
            TotalCount = totalSize
        };
    }

    public async Task<List<TResult>> GetAllAsync<TResult>()
    {
        return await _context.Set<T>()
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<T> GetAsync(int? id)
    {
        var result = await _context.Set<T>().FindAsync(id);
        if (result is null)
        {
            throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
        }

        return result;
    }

    public async Task<TResult> GetAsync<TResult>(int? id)
    {
        var result = await _context.Set<T>().FindAsync(id);
        if (result is null)
        {
            throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
        }

        return _mapper.Map<TResult>(result);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    async Task IRepository<T>.UpdateAsync<TSource>(int id, TSource source)
    {
        if (id != source.Id)
        {
            throw new BadRequestException("Invalid Id used in request");
        }

        var entity = await GetAsync(id);

        if (entity == null)
        {
            throw new NotFoundException(typeof(T).Name, id);
        }

        _mapper.Map(source, entity);
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }
}