namespace Reflection.Tests.TypesForTesting;

public class Employee : IEmployee
{
    public Employee(int id, string name, int age, string role, int annualSalary, int salaryPerYear)
    {
        this.Id = id;
        this.Name = name;
        this.Age = age;
        this.Role = role;
        this.AnnualSalary = annualSalary;
        this.SalaryPerYear = salaryPerYear;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public string Role { get; set; }

    public int AnnualSalary { get; set; }

    public int SalaryPerYear { get; set; }

    public string GetOrganizationName()
    {
        string orgName = "EPAM";
        Console.WriteLine(orgName);
        return orgName;
    }

    public string GetDetails() => $"Id: {this.Id}, Name: {this.Name}, Age: {this.Age}, AnnualSalary: {this.AnnualSalary}";

    public int GetSalaryPerMonth() => this.AnnualSalary / 12;
}
