using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll();
        IEnumerable<T> GetAllEnu();
        IQueryable<T> GetAllFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters);
        IQueryable<T> GetAllFilteredNoTrack(List<string> includes, params Expression<Func<T, bool>>[] filters);
        T GetByFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters);
        T GetByID(Guid id);
        T GetByID(int id);
        void Add(T model);
        void Detach(T model);
        void Update(T model);
        void Update(T model, Guid id);
        void Delete(T model);
        void DeleteFiltered(Expression<Func<T, bool>> filter);
        void Delete(Guid id);
        void Delete(int id);
    }
}
