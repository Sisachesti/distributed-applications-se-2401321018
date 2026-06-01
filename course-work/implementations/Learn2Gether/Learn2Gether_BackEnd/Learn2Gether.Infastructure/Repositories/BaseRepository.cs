using System;
using System.Linq.Expressions;
using Learn2Gether.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learn2Gether.Infastructure.Repositories;

public class BaseRepository<TType, TId> : IBaseRepository<TType, TId> where TType : class
{
    private readonly Learn2GetherDbContext _context;
    private readonly DbSet<TType> _dbSet;

    public BaseRepository(Learn2GetherDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TType>();
    }

    public TType GetById(TId id)
        {
            TType entity = this._dbSet
                .Find(id);

            return entity;
        }

        public async Task<TType> GetByIdAsync(TId id)
        {
            TType entity = await this._dbSet
                .FindAsync(id);

            return entity;
        }

        public TType FirstOrDefault(Func<TType, bool> predicate)
        {
            TType entity = this._dbSet
                .FirstOrDefault(predicate);

            return entity;
        }

        public async Task<TType> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate)
        {
            TType entity = await this._dbSet
                .FirstOrDefaultAsync(predicate);

            return entity;
        }

        public IEnumerable<TType> GetAll()
        {
            return this._dbSet.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await this._dbSet.ToArrayAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return this._dbSet.AsQueryable();
        }

        public void Add(TType item)
        {
            this._dbSet.Add(item);
            this._context.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await this._dbSet.AddAsync(item);
            await this._context.SaveChangesAsync();
        }

        public void AddRange(TType[] items)
        {
            this._dbSet.AddRange(items);
            this._context.SaveChanges();
        }

        public async Task AddRangeAsync(TType[] items)
        {
            await this._dbSet.AddRangeAsync(items);
            await this._context.SaveChangesAsync();
        }

        public bool Delete(TType entity)
        {
            this._dbSet.Remove(entity);
            this._context.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(TType entity)
        {
            this._dbSet.Remove(entity);
            await this._context.SaveChangesAsync();

            return true;
        }

        public bool Update(TType item)
        {
            try
            {
                this._dbSet.Attach(item);
                this._context.Entry(item).State = EntityState.Modified;
                this._context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                this._dbSet.Attach(item);
                this._context.Entry(item).State = EntityState.Modified;
                await this._context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
}
