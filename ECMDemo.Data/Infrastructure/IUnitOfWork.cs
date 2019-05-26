using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMDemo.Data
{
    public interface IUnitOfWork : IDisposable
    {
        //generic class
        //var user = unitOfWork.GetRepository<User>().GetById(Id);
        
        IRepository<T> GetRepository<T>() where T : class;
        int Save();
    }
}
