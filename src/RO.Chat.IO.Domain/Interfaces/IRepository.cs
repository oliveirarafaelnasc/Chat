using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RO.Chat.IO.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity ObterPorId(Guid id);
        List<TEntity> ObterTodos();
        List<TEntity> ObterTodosPaginado(int s, int t);
        List<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate);
        TEntity Adicionar(TEntity obj);
        TEntity Atualizar(TEntity obj);
        void Remover(TEntity obj);
    
    }
}
