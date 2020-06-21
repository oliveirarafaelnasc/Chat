using RO.Chat.IO.Data.Context;
using RO.Chat.IO.Domain.Interfaces;
using System;

namespace RO.Chat.IO.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatContext _context;
        private bool _disposed;

        public UnitOfWork(ChatContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _disposed = false;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}