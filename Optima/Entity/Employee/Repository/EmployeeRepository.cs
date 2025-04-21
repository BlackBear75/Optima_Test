using Optima.Base.Repository;
using Optima.Base;
using Optima.Configuration;

namespace Optima.Entity.Employee.Repository;

public class EmployeeRepository<TDocument> : BaseDataRepository<TDocument>, IEmployeeRepository<TDocument> where TDocument : BaseData
{
    public EmployeeRepository(AppDbContext  databaseConfiguration) : base(databaseConfiguration)
    {
    }
    
}