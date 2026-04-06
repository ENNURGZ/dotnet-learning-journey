namespace NorthwindEmployeeAdoNetService.Tests;

internal sealed class EmployeeAdoNetServiceReadOperationsTests : IDisposable
{
    private DatabaseService databaseService = default!;
    private EmployeeAdoNetService adoNetService = default!;

    [OneTimeSetUp]
    public void Init()
    {
        this.databaseService = new DatabaseService();
        this.databaseService.CreateTables();
        this.databaseService.InitializeTables();
        this.adoNetService = new EmployeeAdoNetService(SqliteFactory.Instance, DatabaseService.ConnectionString);
    }

    [OneTimeTearDown]
    public void Cleanup() => this.Dispose();

    [Test]
    public void GetEmployees_ReturnsEmployeeList()
    {
        var actual = EmployeesDataSource.GetEmployees.ToList();
        IList<Employee> expected = this.adoNetService.GetEmployees();
        Assert.That(actual, Has.Count.EqualTo(expected.Count));
        
        // Compare employees by ID to ensure they match
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.That(actual[i].Id, Is.EqualTo(expected[i].Id));
        }
    }

    [Test]
    public void GetEmployee_EmployeeIsNotExist_ThrowsRepositoryException()
    {
        _ = Assert.Throws<EmployeeServiceException>(() => this.adoNetService.GetEmployee(employeeId: 0));
    }

    [TestCaseSource(typeof(EmployeesDataSource), nameof(EmployeesDataSource.GetEmployees))]
    public void GetEmployee_EmployeeIsExist_ReturnsEmployee(Employee employee)
    {
        Employee actualEmployee = this.adoNetService.GetEmployee(employee.Id);
        Assert.That(actualEmployee, Is.Not.Null);
        Assert.That(actualEmployee.Id, Is.EqualTo(employee.Id));
        Assert.That(actualEmployee.FirstName, Is.EqualTo(employee.FirstName));
        Assert.That(actualEmployee.LastName, Is.EqualTo(employee.LastName));
    }

    public void Dispose()
    {
        this.databaseService.Dispose();
    }
}
