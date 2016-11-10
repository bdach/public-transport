using System;
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services
{
    public interface ICrudService<T> : IDisposable
        where T : Entity
    {
        T Create(T entity);
        T Read(int id);
        T Update(T entity);
        void Delete(T entity);
    }
}