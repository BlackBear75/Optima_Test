
using Optima.Base;
using Optima.Base.Repository;

namespace Optima.Entity.Employee.Repository;

public interface IEmployeeRepository<TDocument> : IBaseDataRepository<TDocument> where TDocument : BaseData
{
   
}