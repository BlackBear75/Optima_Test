using Moq;
using Optima.Entity.Employee;
using Optima.Entity.Employee.Repository;
using Optima.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository<Employee>> _employeeRepositoryMock;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository<Employee>>();
        _employeeService = new EmployeeService(_employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmployees()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new Employee { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        _employeeRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);

        // Act
        var result = await _employeeService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task InsertOneAsync_ShouldCallInsertOnce()
    {
        // Arrange
        var newEmployee = new Employee { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

        // Act
        await _employeeService.InsertOneAsync(newEmployee);

        // Assert
        _employeeRepositoryMock.Verify(repo => repo.InsertOneAsync(It.Is<Employee>(e => e.Id == newEmployee.Id)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteOnce()
    {
        // Arrange
        var employeeId = Guid.NewGuid();

        // Act
        await _employeeService.DeleteAsync(employeeId);

        // Assert
        _employeeRepositoryMock.Verify(repo => repo.DeleteOneAsync(It.Is<Guid>(id => id == employeeId)), Times.Once);
    }

    [Fact]
    public async Task UpdateOneAsync_ShouldCallUpdateOnce()
    {
        // Arrange
        var existingEmployee = new Employee { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

        // Act
        await _employeeService.UpdateOneAsync(existingEmployee);

        // Assert
        _employeeRepositoryMock.Verify(repo => repo.UpdateOneAsync(It.Is<Employee>(e => e.Id == existingEmployee.Id)), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldReturnEmployee()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new Employee { Id = employeeId, FirstName = "John", LastName = "Doe" };

        _employeeRepositoryMock.Setup(repo => repo.FindByIdAsync(employeeId)).ReturnsAsync(employee);

        // Act
        var result = await _employeeService.FindByIdAsync(employeeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeId, result.Id);
    }

    [Fact]
    public void Detach_ShouldCallDetachOnce()
    {
        // Arrange
        var employee = new Employee { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

        // Act
        _employeeService.Detach(employee);

        // Assert
        _employeeRepositoryMock.Verify(repo => repo.Detach(It.Is<Employee>(e => e.Id == employee.Id)), Times.Once);
    }
}
