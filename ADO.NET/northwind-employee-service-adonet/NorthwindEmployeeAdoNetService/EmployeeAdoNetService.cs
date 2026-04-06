namespace NorthwindEmployeeAdoNetService;

/// <summary>
/// A service for interacting with the "Employees" table using ADO.NET.
/// </summary>
public sealed class EmployeeAdoNetService
{
    private readonly DbProviderFactory dbFactory;
    private readonly string connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeAdoNetService"/> class.
    /// </summary>
    /// <param name="dbFactory">The database provider factory used to create database connection and command instances.</param>
    /// <param name="connectionString">The connection string used to establish a database connection.</param>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="dbFactory"/> or <paramref name="connectionString"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="connectionString"/> is empty or contains only white-space characters.</exception>
    public EmployeeAdoNetService(DbProviderFactory dbFactory, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(dbFactory);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }

        this.dbFactory = dbFactory;
        this.connectionString = connectionString;
    }

    /// <summary>
    /// Retrieves a list of all employees from the Employees table of the database.
    /// </summary>
    /// <returns>A list of Employee objects representing the retrieved employees.</returns>
    public IList<Employee> GetEmployees()
    {
        var employees = new List<Employee>();

        using var connection = this.dbFactory.CreateConnection();
        if (connection == null)
        {
            return employees;
        }

        connection.ConnectionString = this.connectionString;
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Employees";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            employees.Add(MapEmployee(reader));
        }

        return employees;
    }

    private static Employee MapEmployee(DbDataReader reader)
    {
        var id = reader.GetInt64(reader.GetOrdinal("EmployeeID"));
        var employee = new Employee(id)
        {
            LastName = reader.GetString(reader.GetOrdinal("LastName")),
            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
            Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
            TitleOfCourtesy = reader.IsDBNull(reader.GetOrdinal("TitleOfCourtesy")) ? null : reader.GetString(reader.GetOrdinal("TitleOfCourtesy")),
            BirthDate = reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
            HireDate = reader.IsDBNull(reader.GetOrdinal("HireDate")) ? null : reader.GetDateTime(reader.GetOrdinal("HireDate")),
            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
            City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
            Region = reader.IsDBNull(reader.GetOrdinal("Region")) ? null : reader.GetString(reader.GetOrdinal("Region")),
            PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? null : reader.GetString(reader.GetOrdinal("PostalCode")),
            Country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country")),
            HomePhone = reader.IsDBNull(reader.GetOrdinal("HomePhone")) ? null : reader.GetString(reader.GetOrdinal("HomePhone")),
            Extension = reader.IsDBNull(reader.GetOrdinal("Extension")) ? null : reader.GetString(reader.GetOrdinal("Extension")),
            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
            ReportsTo = reader.IsDBNull(reader.GetOrdinal("ReportsTo")) ? null : reader.GetInt64(reader.GetOrdinal("ReportsTo")),
            PhotoPath = reader.IsDBNull(reader.GetOrdinal("PhotoPath")) ? null : reader.GetString(reader.GetOrdinal("PhotoPath")),
        };

        return employee;
    }

    /// <summary>
    /// Retrieves an employee with the specified employee ID.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to retrieve.</param>
    /// <returns>The retrieved an <see cref="Employee"/> instance.</returns>
    /// <exception cref="EmployeeServiceException">Thrown if the employee is not found.</exception>
    public Employee GetEmployee(long employeeId)
    {
        using var connection = this.dbFactory.CreateConnection();
        if (connection == null)
        {
            throw new EmployeeServiceException("Database connection creation failed.");
        }

        connection.ConnectionString = this.connectionString;
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Employees WHERE EmployeeID = @id";

        var idParameter = command.CreateParameter();
        idParameter.ParameterName = "@id";
        idParameter.Value = employeeId;
        _ = command.Parameters.Add(idParameter);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapEmployee(reader);
        }

        throw new EmployeeServiceException("Employee not found.");
    }

    /// <summary>
    /// Adds a new employee to Employee table of the database.
    /// </summary>
    /// <param name="employee">The  <see cref="Employee"/> object containing the employee's information.</param>
    /// <returns>The ID of the newly added employee.</returns>
    /// <exception cref="EmployeeServiceException">Thrown when an error occurs while adding the employee.</exception>
    public long AddEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        using var connection = this.dbFactory.CreateConnection();
        if (connection == null)
        {
            throw new EmployeeServiceException("Database connection creation failed.");
        }

        connection.ConnectionString = this.connectionString;
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = 
            "INSERT INTO Employees (LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Notes, ReportsTo, PhotoPath) " +
            "VALUES (@LastName, @FirstName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, @Address, @City, @Region, @PostalCode, @Country, @HomePhone, @Extension, @Notes, @ReportsTo, @PhotoPath); " +
            "SELECT last_insert_rowid();";

        AddParameter(command, "@LastName", employee.LastName);
        AddParameter(command, "@FirstName", employee.FirstName);
        AddParameter(command, "@Title", (object?)employee.Title ?? DBNull.Value);
        AddParameter(command, "@TitleOfCourtesy", (object?)employee.TitleOfCourtesy ?? DBNull.Value);
        AddParameter(command, "@BirthDate", (object?)employee.BirthDate ?? DBNull.Value);
        AddParameter(command, "@HireDate", (object?)employee.HireDate ?? DBNull.Value);
        AddParameter(command, "@Address", (object?)employee.Address ?? DBNull.Value);
        AddParameter(command, "@City", (object?)employee.City ?? DBNull.Value);
        AddParameter(command, "@Region", (object?)employee.Region ?? DBNull.Value);
        AddParameter(command, "@PostalCode", (object?)employee.PostalCode ?? DBNull.Value);
        AddParameter(command, "@Country", (object?)employee.Country ?? DBNull.Value);
        AddParameter(command, "@HomePhone", (object?)employee.HomePhone ?? DBNull.Value);
        AddParameter(command, "@Extension", (object?)employee.Extension ?? DBNull.Value);
        AddParameter(command, "@Notes", (object?)employee.Notes ?? DBNull.Value);
        AddParameter(command, "@ReportsTo", (object?)employee.ReportsTo ?? DBNull.Value);
        AddParameter(command, "@PhotoPath", (object?)employee.PhotoPath ?? DBNull.Value);

        try
        {
            var result = command.ExecuteScalar();
            return result != null ? (long)result : throw new EmployeeServiceException("Inserting an employee failed.");
        }
        catch (Exception ex) when (ex is not EmployeeServiceException)
        {
            throw new EmployeeServiceException("Inserting an employee failed.", ex);
        }
    }

    private void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        _ = command.Parameters.Add(parameter);
    }

    /// <summary>
    /// Removes an employee from the the Employee table of the database based on the provided employee ID.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to remove.</param>
    /// <exception cref="EmployeeServiceException"> Thrown when an error occurs while attempting to remove the employee.</exception>
    public void RemoveEmployee(long employeeId)
    {
        using var connection = this.dbFactory.CreateConnection();
        if (connection == null)
        {
            throw new EmployeeServiceException("Database connection creation failed.");
        }

        connection.ConnectionString = this.connectionString;
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Employees WHERE EmployeeID = @id";

        AddParameter(command, "@id", employeeId);

        try
        {
            _ = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new EmployeeServiceException("Removing an employee failed.", ex);
        }
    }

    /// <summary>
    /// Updates an employee record in the Employee table of the database.
    /// </summary>
    /// <param name="employee">The employee object containing updated information.</param>
    /// <exception cref="EmployeeServiceException">Thrown when there is an issue updating the employee record.</exception>
    public void UpdateEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        using var connection = this.dbFactory.CreateConnection();
        if (connection == null)
        {
            throw new EmployeeServiceException("Database connection creation failed.");
        }

        connection.ConnectionString = this.connectionString;
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = 
            "UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, Title = @Title, TitleOfCourtesy = @TitleOfCourtesy, BirthDate = @BirthDate, HireDate = @HireDate, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, HomePhone = @HomePhone, Extension = @Extension, Notes = @Notes, ReportsTo = @ReportsTo, PhotoPath = @PhotoPath " +
            "WHERE EmployeeID = @id";

        AddParameter(command, "@id", employee.Id);
        AddParameter(command, "@LastName", employee.LastName);
        AddParameter(command, "@FirstName", employee.FirstName);
        AddParameter(command, "@Title", (object?)employee.Title ?? DBNull.Value);
        AddParameter(command, "@TitleOfCourtesy", (object?)employee.TitleOfCourtesy ?? DBNull.Value);
        AddParameter(command, "@BirthDate", (object?)employee.BirthDate ?? DBNull.Value);
        AddParameter(command, "@HireDate", (object?)employee.HireDate ?? DBNull.Value);
        AddParameter(command, "@Address", (object?)employee.Address ?? DBNull.Value);
        AddParameter(command, "@City", (object?)employee.City ?? DBNull.Value);
        AddParameter(command, "@Region", (object?)employee.Region ?? DBNull.Value);
        AddParameter(command, "@PostalCode", (object?)employee.PostalCode ?? DBNull.Value);
        AddParameter(command, "@Country", (object?)employee.Country ?? DBNull.Value);
        AddParameter(command, "@HomePhone", (object?)employee.HomePhone ?? DBNull.Value);
        AddParameter(command, "@Extension", (object?)employee.Extension ?? DBNull.Value);
        AddParameter(command, "@Notes", (object?)employee.Notes ?? DBNull.Value);
        AddParameter(command, "@ReportsTo", (object?)employee.ReportsTo ?? DBNull.Value);
        AddParameter(command, "@PhotoPath", (object?)employee.PhotoPath ?? DBNull.Value);

        try
        {
            var rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new EmployeeServiceException("Employee is not updated.");
            }
        }
        catch (Exception ex) when (ex is not EmployeeServiceException)
        {
            throw new EmployeeServiceException("Employee is not updated.", ex);
        }
    }
}
