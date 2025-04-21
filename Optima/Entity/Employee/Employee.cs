using Optima.Base;

namespace Optima.Entity.Employee;

public class Employee : BaseData
{
    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public double Salary { get; set; }

    public bool Liberated { get; set; }
    public EmployeePosition Position { get; set; }


    public string GetFullName()
    {
        return $"{FirstName} {MiddleName} {LastName}";
    }
}


public enum EmployeePosition
{
    JuniorDeveloper ,
    SeniorDeveloper,
    QAEngineer,
    TeamLead,
    ProjectManager,
    CTO
}
