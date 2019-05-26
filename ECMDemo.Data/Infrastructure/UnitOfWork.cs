using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ECMDemo.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private Entities _dataContext;
        private bool disposed;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            // _databaseFactory = databaseFactory;
            _databaseFactory = new DatabaseFactory();
            _dataContext = _databaseFactory.GetDbContext();
        }

        public UnitOfWork()
        {
            _databaseFactory = new DatabaseFactory();
            _dataContext = _databaseFactory.GetDbContext();
        }

        public Entities DataContext
        {
            get { return _dataContext ?? (_dataContext = _databaseFactory.GetDbContext()); }
        }

        public IRepository<T> GetRepository<T>() where T: class
        {
            return new Repository<T>(this._dataContext);
        }

        public int Save()
        {
            if (_dataContext.GetValidationErrors().Any())
            {
                throw (new Exception(_dataContext.GetValidationErrors().ToList()[0].ValidationErrors.ToList()[0].ErrorMessage)) ;
            }
            return DataContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dataContext.Dispose();
                    disposed = true;
                }
            }
            disposed = false;
        }

    }
}
