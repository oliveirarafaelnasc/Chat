using RO.Chat.IO.Data.Context;
using RO.Chat.IO.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RO.Chat.IO.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ChatContext Db;
        protected DbSet<TEntity> DbSet;

        protected Repository(ChatContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }
        public TEntity Adicionar(TEntity obj)
        {
            DbSet.Add(obj);

            return obj;
        }

        public TEntity Atualizar(TEntity obj)
        {
            DbSet.Add(obj);
            Db.Entry<TEntity>(obj).State = System.Data.Entity.EntityState.Modified;

            return obj;
        }
        public void Remover(TEntity obj)
        {
            DbSet.Add(obj);
            Db.Entry<TEntity>(obj).State = System.Data.Entity.EntityState.Deleted;
        }

        public List<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return new List<TEntity>();
        }

        public TEntity ObterPorId(Guid id)
        {
            return DbSet.Find(id);
        }

        public List<TEntity> ObterTodos()
        {
            return DbSet.ToList();
        }

        public List<TEntity> ObterTodosPaginado(int s, int t)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
