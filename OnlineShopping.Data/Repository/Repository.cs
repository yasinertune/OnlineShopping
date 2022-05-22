using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context; 
            _dbSet = _context.Set<T>();
        }


        public void Add(T entity)
        {
           _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            //bu alanda kayıtlar dönüyor
            IQueryable<T> query = _dbSet;

            if (filter!=null)
            {
                //x=> x.id == 1
                query=query.Where(filter); //buradan gelen expression func komutunu where içine yazdık. Yani filter onu temsil ediyor
            }

            if(includeProperties!=null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
         }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            //bu alanda tek kayıt dönüyor
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                //x=> x.id == 1
                query = query.Where(filter); //buradan gelen expression func komutunu where içine yazdık. Yani filter onu temsil ediyor
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
        }


        public void Remove(T entity)
        {//tek bir kayıt silme
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {//birden fazla kayıt silme
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
