using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.Controllers
{
    public interface IBaseController<T, TID>
    {
        IEnumerable<T> GetAll();

        T Get(TID id);

        void Post([FromBody]T entity);

        void Put([FromBody]T entity);

        void Delete(T entity);
    }
}