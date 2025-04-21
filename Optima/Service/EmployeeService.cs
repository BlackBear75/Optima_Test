using Microsoft.EntityFrameworkCore;
using Optima.Entity.Employee;
using Optima.Entity.Employee.Repository;
using System.Linq.Expressions;

namespace Optima.Service;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync();

    Task InsertOneAsync(Employee employee);
    Task DeleteAsync(Guid id);
    void Detach(Employee employee);
    
     Task<Employee> FindByIdAsync(Guid id);

    Task UpdateOneAsync(Employee employee);
}

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository<Employee> _employeeRepository;

    public EmployeeService(IEmployeeRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }


        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
            
        }

        public async Task InsertOneAsync(Employee employee)
        {
            await _employeeRepository.InsertOneAsync(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _employeeRepository.DeleteOneAsync(id);
        }

    public async Task UpdateOneAsync(Employee employee)
    {
       await _employeeRepository.UpdateOneAsync(employee);
    }

  


    public async Task<Employee> FindByIdAsync(Guid id)
    {
      return  await _employeeRepository.FindByIdAsync(id);
    }
    public void Detach(Employee employee)
    {
      _employeeRepository.Detach(employee);
    }

}



